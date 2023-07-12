using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class Rating
    {
        private const string ElementName = "Rating";
        private const string SoundIdAttributeName = "SoundId";
        private const string ResultAttributeName = "Result";

        public string SoundId;
        public string Result;

        public Rating(XElement element)
        {
            load(element);
        }

        public Rating(ISoundReference sound)
        {
            SoundId = sound.Id;
            Result = string.Empty;
        }

        public XElement Save()
        {
            var element = new XElement(ElementName);
            element.Add(new XAttribute(SoundIdAttributeName, SoundId));
            element.Add(new XAttribute(ResultAttributeName, Result));
            return element;
        }

        private void load(XElement element)
        {
            SoundId = element.Attribute(SoundIdAttributeName)?.Value;
            Result = element.Attribute(ResultAttributeName)?.Value;
        }
    }
}