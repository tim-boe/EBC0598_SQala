using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class Step : IStep, INotifyPropertyChanged
    {
        private StepConfig config;
        private List<ISoundReference> sounds;
        private DefaultNavigator navigator;
        private List<Rating> results;

        public IPlayer Player { get; set; }
        public IAttributeReference Attribute { get; private set; }
        public ISoundReference ReferenceSound { get; private set; }

        public ISoundReference CurrentSound => sounds[navigator.CurrentIndex];
        public string CurrentResult
        {
            get => results.Count > navigator.CurrentIndex ? results[navigator.CurrentIndex].Result : string.Empty;
            set
            {
                results[navigator.CurrentIndex].Result = value;
                //once the result is set we can enable navigation to the next sound
                navigator.EnableNextOrFinish();
            }
        }

        private bool attributeEnabled;
        public bool AttributeEnabled
        {
            get => attributeEnabled;
            set
            {
                if (value != attributeEnabled)
                {
                    attributeEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttributeEnabled)));
                }
            }
        }

        public void OnStarted(XElement readonlyStepConfig, IRuntimeEnvironment environment)
        {
            config = new StepConfig(readonlyStepConfig);

            Attribute = environment.AttributeReferences.ToList().Find(a => a.Id == config.SelectedAttributeId);
            sounds = environment.SoundReferences.ToList();
            ReferenceSound = sounds.Find(a => a.Id == config.ReferenceSoundId);
            sounds.Remove(ReferenceSound);

            navigator = new DefaultNavigator(environment.Navigation, sounds.Count, true);
            navigator.NavigateNextClicked += onNavigateNextClicked;
            navigator.NavigatePreviousClicked += onNavigatePreviousClicked;

            results = new List<Rating>();
            foreach (var soundReference in sounds)
            {
                results.Add(new Rating(soundReference));
            }

            Player = environment.Player;
            Player.OnPlaybackEvent += onPlaybackEvent;

            View = new StepView { DataContext = this };
        }


        public void OnCanceled()
        {
            Player.Stop();
        }

        public StepResult OnFinished()
        {
            var element = new XElement("Results");
            foreach (var result in results)
            {
                element.Add(result.Save());
            }
            return new StepResult(element);
        }

        public FrameworkElement View { get; private set; }

        private void onPlaybackEvent(object sender, PlaybackEventArgs playbackEventArgs)
        {
            //if a sound that was not the reference finished playing -> we enable the attributeControl to enable rating
            if (playbackEventArgs.Type == PlaybackEventType.Finished && playbackEventArgs.SoundReference.Id != ReferenceSound.Id)
                AttributeEnabled = true;
        }

        private void onNavigateNextClicked(object sender, EventArgs eventArgs)
        {
            Player.Stop();
            //if the result was already set  in the past
            if (!string.IsNullOrEmpty(CurrentResult))
            {
                //enable rating and allow the user to continue
                AttributeEnabled = true;
                navigator.EnableNextOrFinish();
            }
            else
            {
                //disable the attributeControl to ensure user has to listen to the sound first
                AttributeEnabled = false;
            }
            //inform the UI that sound and result may have changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentResult)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSound)));
        }

        private void onNavigatePreviousClicked(object sender, EventArgs eventArgs)
        {
            Player.Stop();
            //since we already rated all previous elements we can enable the attributeControl outright
            AttributeEnabled = true;
            //inform the UI that sound and result may have changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentResult)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSound)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}