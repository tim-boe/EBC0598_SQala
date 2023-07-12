using System;
using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class StepConfig : IStepConfig, IValidatable
    {
        private const string ExampleStepConfigElementName = "ExampleStepConfig";
        private const string SelectedAttributeIdElementName = "SelectedAttributeId";
        private const string ReferenceSoundIdName = "ReferenceSoundId";

        public StepConfig(XElement element)
        {
            Load(element);
        }

        public StepConfig() { }

        public string SelectedAttributeId { get; set; }
        public string ReferenceSoundId { get; set; }

        #region IStepConfig
        public XElement Save()
        {
            var element = new XElement(ExampleStepConfigElementName);

            //here we save the desired attribute id to our xElement for it to be included in the SQala project file
            element.Add(new XElement(SelectedAttributeIdElementName, SelectedAttributeId));
            element.Add(new XElement(ReferenceSoundIdName, ReferenceSoundId));
            return element;
        }

        public void Load(XElement xElement)
        {
            //During loading of a project file SQala will supply us with the matching xElement that we did supply in the SaveToXml method
            var xSelectedAttribute = xElement.Element(SelectedAttributeIdElementName);
            if (xSelectedAttribute != null)
                SelectedAttributeId = xSelectedAttribute.Value;

            var xReferenceSoundId = xElement.Element(ReferenceSoundIdName);
            if (xReferenceSoundId != null)
                ReferenceSoundId = xReferenceSoundId.Value;
        }
        #endregion

        #region IValidatable
        public bool IsValid()
        {
            bool isValid = true;

            //reset the errorMessage
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(SelectedAttributeId))
            {
                //we did not yet set an Attribute, lets give a message to the user
                ErrorMessage += "Please select an attribute" + Environment.NewLine;
                isValid = false;
            }

            if (string.IsNullOrEmpty(ReferenceSoundId))
            {
                //we did not yet set a reference sound, lets give a message to the user
                ErrorMessage += "Please select a reference sound" + Environment.NewLine;
                isValid = false;
            }

            //attribute was set, config is valid
            return isValid;
        }

        public string ErrorMessage { get; private set; }
        #endregion
    }
}