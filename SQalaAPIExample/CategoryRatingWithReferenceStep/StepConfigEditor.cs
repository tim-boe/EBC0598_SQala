using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class StepConfigEditor : IStepConfigEditor
    {
        private StepConfig config;

        public string Title => "Example Step Properties";
        public UserControl View { get; set; }

        public List<IAttributeReference> Attributes { get; private set; }
        public List<ISoundReference> Sounds { get; private set; }

        public IAttributeReference SelectedAttribute
        {
            //Restore the attribute by finding the stored id within the available attributes
            get
            {
                var desiredAttribute = Attributes.Find(a => a.Id == config.SelectedAttributeId);
                return desiredAttribute;
            }
            //Save the attribute by storing its id
            set
            {
                if (config.SelectedAttributeId != value.Id)
                {
                    config.SelectedAttributeId = value.Id;
                    // Changing the AttributeId may change the validity or the visualization of the editor view and should therefore inform the framework
                    ConfigChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public ISoundReference ReferenceSound
        {
            //Restore the sound by finding the stored id within the available attributes
            get
            {
                var desiredSound = Sounds.Find(a => a.Id == config.ReferenceSoundId);
                return desiredSound;
            }
            //Save the sound by storing its id
            set
            {
                if (config.ReferenceSoundId != value.Id)
                {
                    config.ReferenceSoundId = value.Id;
                    // Changing the SoundId may change the validity or the visualization of the editor view and should therefore inform the framework
                    ConfigChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public StepConfigEditor(StepConfig stepConfig, IEditorEnvironment environment)
        {
            config = stepConfig;
            Attributes = environment.AttributeReferences.ToList();
            Sounds = environment.SoundReferences.ToList();
            View = new StepConfigEditorView() { DataContext = this };
        }

        public void Dispose()
        {
            View = null;
            config = null;
            Attributes = null;
            Sounds = null;
        }

        public event EventHandler ConfigChanged;
    }
}