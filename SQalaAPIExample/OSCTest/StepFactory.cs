using HEADacoustics.API.SQala;

namespace OSCTestStep
{
    [ExportStepFactory("OSCTestStep")]
    public class OSCTestFactory : IStepFactory
    {
        public FactorySettings Settings
        {
            get
            {
                var settings = new FactorySettings();
                settings.SetName("OCS Test Step");
                settings.SetCategory("API Documentation");
                settings.SetUsesSounds(true);
                settings.SetMinimumNumberOfSounds(1);
                settings.SetMaximumNumberOfSounds(1);
                return settings;
            }
        }
        public IStep CreateStep() => new OSCTestStep();
    }
}