import argparse
import pyads
from pythonosc.dispatcher import Dispatcher
from pythonosc import osc_server


def osc_handler(address, *args):
    # Write to the PLC global variable
    value = args[0]  # Assuming a single value is sent in the OSC message
    data_type = args[1]
    data_type = get_plc_data_type(data_type)
    address = address[1:]
    # address = address[:-1]

    print("address: ", address)
    print("value: ", str(value))

    try:
        plc.write_by_name(address, value, data_type)
    except Exception as e:
        print(e)
        print("Failed to write to PLC variable.")


def get_plc_data_type(plc_type):
    if plc_type == "BOOL":
        plc_type = pyads.PLCTYPE_BOOL
    elif plc_type == "REAL":
        plc_type = pyads.PLCTYPE_REAL
    elif plc_type == "INT":
        plc_type = pyads.PLCTYPE_INT
    elif plc_type == "DT":
        plc_type = pyads.PLCTYPE_DT
    elif plc_type == "STRING":
        plc_type = pyads.PLCTYPE_STRING
    elif plc_type == "DWORD":
        plc_type = pyads.PLCTYPE_DWORD
    return plc_type


if __name__ == "__main__":
    plc_ip = '5.78.127.222.1.1'
    plc_net_id = 851
    plc_var_name = "GVL_Illum.fbDaliLight[1].fLightLevelSet"  # Replace with your PLC's global variable name

    plc = pyads.Connection(plc_ip, plc_net_id)
    plc.open()

    # asyncio.run(init_main())
    parser = argparse.ArgumentParser()
    parser.add_argument("--ip",
                        default="127.0.0.1", help="The ip to listen on")
    parser.add_argument("--port",
                        type=int, default=55555, help="The port to listen on")
    args = parser.parse_args()

    dispatcher = Dispatcher()
    dispatcher.set_default_handler(osc_handler)
    server = osc_server.ThreadingOSCUDPServer(
        (args.ip, args.port), dispatcher)
    print("Serving on {}".format(server.server_address))
    server.serve_forever()
