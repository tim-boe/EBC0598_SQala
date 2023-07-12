import argparse
import math
import pyads

from pythonosc.dispatcher import Dispatcher
from pythonosc.osc_server import AsyncIOOSCUDPServer
from pythonosc import osc_server
import asyncio
import time
import datetime


# dispatcher = Dispatcher()
# dispatcher.map("/filter", filter_handler)

# ip = "127.0.0.1"
# port = 55555
#
#
# class MyAds:
#     def __init__(self, symbol_list_file, ams_net_id, ams_net_port=851, logger=False, ctrl_sched_file=None):
#         self.symbol_list_file = symbol_list_file
#         self.symbol_list_file_autogen = 'AdsSymList_AutoGen.txt'
#         self.ams_net_id = ams_net_id
#         self.ams_net_port = ams_net_port
#         self.plc = pyads.Connection(ams_net_id, ams_net_port)
#         self.plc.open()
#         self.state = self.plc.read_state()
#         print(f"PLC read state: {self.state}")
#
#     def write(self, name, value, plc_type):
#         if plc_type == "BOOL":
#             plc_type = pyads.PLCTYPE_BOOL
#         elif plc_type == "REAL":
#             plc_type = pyads.PLCTYPE_REAL
#         elif plc_type == "INT":
#             plc_type = pyads.PLCTYPE_INT
#         elif plc_type == "DT":
#             plc_type = pyads.PLCTYPE_DT
#         elif plc_type == "STRING":
#             plc_type = pyads.PLCTYPE_STRING
#         elif plc_type == "DWORD":
#             plc_type = pyads.PLCTYPE_DWORD
#         # write to the plc
#         self.plc.write_by_name(name, value, plc_type)
#
#
# # def print_volume_handler(unused_addr, args, volume):
# #     print("[{0}] ~ {1}".format(args[0], volume))
# #
# #
# # def print_compute_handler(unused_addr, args, volume):
# #     try:
# #         print("[{0}] ~ {1}".format(args[0], args[1](volume)))
# #     except ValueError:
# #         pass
#
#
# # async def loop():
# #     """Main ADS loop"""
# #     try:
# #         myPlc = MyAds(symbol_list_file='01_LogVarList.txt', ams_net_id='5.78.127.222.1.1', ams_net_port=851,
# #                       logger=False,
# #                       ctrl_sched_file=None)  # Set ctrl_sched_file to None to deactivate the function
# #         # while True:
# #         #     t1 = time.time()
# #         #     myPlc.run()
# #         #     # apply the interval
# #         #     t2 = time.time()
# #         #     delta_t = t2 - t1
# #         #     print('delta_t = ' + str(delta_t))
# #     except KeyboardInterrupt:
# #         myPlc.plc.close()
# #         # myPlc.plc.release_handle(myPlc.var_handle)
# #         print('\nPLC closed, handle released.')
#
#
# # async def init_main():
# #     server = AsyncIOOSCUDPServer((ip, port), dispatcher, asyncio.get_event_loop())
# #     transport, protocol = await server.create_serve_endpoint()  # Create datagram endpoint and start serving
# #
# #     await loop()  # Enter main loop of program
# #
# #     transport.close()  # Clean up serve endpoint
#
# def filter_handler(address, *args):
#     print(f"{address}: {args}")
#
# def handle_bool(ads):
#     ads.write()
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
