using HEADacoustics.API.SQala;

namespace HelloWorldStep
{
    [ExportStepFactory("HelloWorldStep")]
    public class HelloWorldFactory : IStepFactory
    {
        public FactorySettings Settings
        {
            get
            {
                var settings = new FactorySettings();
                settings.SetName("Hello World Step");
                settings.SetCategory("API Documentation");
                settings.SetUsesSounds(true);
                settings.SetMinimumNumberOfSounds(1);
                settings.SetMaximumNumberOfSounds(1);
                return settings;
            }
        }
        public IStep CreateStep() => new HelloWorldStep();
    }
}