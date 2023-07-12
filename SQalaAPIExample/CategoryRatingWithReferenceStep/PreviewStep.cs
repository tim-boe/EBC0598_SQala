using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class StepPreviewDataContext
    {
        public IAttributeReference Attribute { get; }

        public StepPreviewDataContext(IAttributeReference attribute)
        {
            Attribute = attribute;
        }
    }
}