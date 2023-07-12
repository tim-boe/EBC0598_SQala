using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using HEADacoustics.API.SQala;
using SharpOSC;


namespace OSCTestStep
{
    public class OSCTestStep : IStep
    {
        public ISoundReference Sound { get; set; }
        public IPlayer Player { get; set; }
        private INavigation navigation;

        public void OnStarted(XElement readonlyStepConfig, IRuntimeEnvironment environment)
        {
            Sound = environment.SoundReferences.First();
            Player = environment.Player;
            navigation = environment.Navigation;
            Player.OnPlaybackEvent += onPlaybackEvent;
        }

        public void OnCanceled() { }
        public StepResult OnFinished() => null;

        private void onPlaybackEvent(object sender, PlaybackEventArgs e)
        {
            if (e.Type == PlaybackEventType.Finished)
                navigation.FinishEnabled = true;
            var oscSender = new OSCSender();
            if (e.Type == PlaybackEventType.Finished) 
                oscSender.SendOSCMessage("/GVL_Illum.fbDaliLight[1].fLightLevelSet", 0.9, "REAL"); //  (osc ardress and plc symbol name, value, PLC_DTYPE)  
            if (e.Type != PlaybackEventType.Finished)
                oscSender.SendOSCMessage("/GVL_Illum.fbDaliLight[1].fLightLevelSet", 0.0, "REAL");   // (osc ardress and plc symbol name, value)

        }

        public FrameworkElement View => new StepView() { DataContext = this };
    }

    class OSCSender
    {
        public void SendOSCMessage(string adress, object data, string dtype)
        {
            var message = new SharpOSC.OscMessage(adress, data, dtype);
            var sender = new SharpOSC.UDPSender("127.0.0.1", 55555);
            sender.Send(message);
        }
    }
}