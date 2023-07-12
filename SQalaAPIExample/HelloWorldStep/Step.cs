using System.Linq;
using System.Windows;
using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace HelloWorldStep
{
    public class HelloWorldStep : IStep
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

        }

        public FrameworkElement View => new StepView() { DataContext = this };
    }
}