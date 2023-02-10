using EA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JaackdEAAddin
{
    internal class PreNewObjectService : IPreNewObjectService
    {
        private readonly ILogger<PreNewObjectService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEAService _eaService;
        private readonly IMDGService _mdgService;

        public PreNewObjectService(ILogger<PreNewObjectService> logger, IConfiguration configuration, IMDGService mdgService, IEAService eaService)
        {
            _logger = logger;
            _configuration = configuration;
            _mdgService = mdgService;
            _eaService = eaService;
        }

        #region EA Pre-New-Object Events

        /// <summary>
        /// OnPreNewElement notifies Add-Ins that a new element is about to be created on a diagram. It enables Add-Ins to permit or deny creation of the new element.
        /// This event occurs when a user drags a new element from the Toolbox or Resources window onto a diagram. 
        /// The notification is provided immediately before the element is created, so that the Add-In can disable addition of the element.
        /// Also look at EA_OnPostNewElement.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the element to be created:
        /// - Type: A string value corresponding to Element.Type
        /// - Stereotype: A string value corresponding to Element.Stereotype
        /// - ParentID: A long value corresponding to Element.ParentID
        /// - DiagramID: A long value corresponding to the ID of the diagram to which the element is being added. </param>
        /// <returns>Return True to enable addition of the new element to the model. Return False to disable addition of the new element.</returns>
        public virtual bool OnPreNewElement(EA.Repository repository, EA.EventProperties info) {
            _logger.LogInformation("OnPreNewElement");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// OnPreNewConnector notifies Add-Ins that a new connector is about to be created on a diagram. It enables Add-Ins to permit or deny creation of a new connector.
        /// This event occurs when a user drags a new connector from the Toolbox or Resources window, onto a diagram. The notification is provided immediately before the connector is created, so that the Add-In can disable addition of the connector.
        /// Also look at OnPostNewConnector.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the connector to be created:
        /// - Type: A string value corresponding to Connector.Type
        /// - Subtype: A string value corresponding to Connector.Subtype
        /// - Stereotype: A string value corresponding to Connector.Stereotype
        /// - ClientID: A long value corresponding to Connector.ClientID
        /// - SupplierID: A long value corresponding to Connector.SupplierID
        /// - DiagramID: A long value corresponding to Connector.DiagramID.
        /// </param>
        /// <returns>Return True to enable addition of the new connector to the model. Return False to disable addition of the new connector.</returns>
        public virtual bool OnPreNewConnector(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewConnector");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// OnPreNewDiagram notifies Add-Ins that a new diagram is about to be created. It enables Add-Ins to permit or deny creation of the new diagram.
        /// The notification is provided immediately before the diagram is created, so that the Add-In can disable addition of the diagram.
        /// Also look at OnPostNewDiagram.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the diagram to be created:
        /// - Type: A string value corresponding to Diagram.Type
        /// - ParentID: A long value corresponding to Diagram.ParentID
        /// - PackageID: A long value corresponding to Diagram.PackageID. </param>
        /// <returns>Return True to enable addition of the new diagram to the model. Return False to disable addition of the new diagram.</returns>
        public virtual bool OnPreNewDiagram(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewDiagram");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// OnPreNewDiagramObject notifies Add-Ins that a new diagram object is about to be dropped on a diagram. It enables Add-Ins to permit or deny creation of the new object.
        /// This event occurs when a user drags an object from the Enterprise Architect Project Browser or Resources window onto a diagram. The notification is provided immediately before the object is created, so that the Add-In can disable addition of the object.
        /// Also look at OnPostNewDiagramObject.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the object to be created:
        /// - Type: A string value corresponding to Object.Type
        /// - Stereotype: A string value corresponding to Object.Stereotype
        /// - ParentID: A long value corresponding to Object.ParentID
        /// - DiagramID: A long value corresponding to the ID of the diagram to which the object is being added. </param>
        /// <returns>Return True to enable addition of the object to the model. Return False to disable addition of the object.</returns>
        public virtual bool OnPreNewDiagramObject(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewDiagramObject");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// When a user drags any kind of element from the Project Browser onto a diagram, OnPreDropFromTree notifies the Add-In that a new item is about to be dropped onto a diagram. The notification is provided immediately before the element is dropped, so that the Add-In can override the default action that would be taken for this drag.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the element to be created:
        /// · ID: A long value of the type being dropped
        /// · Type: A string value corresponding to type of element being dropped
        /// · DiagramID: A long value corresponding to the ID of the diagram to which the element is being added
        /// · PositionX: The X coordinate into which the element is being dropped
        /// · PositionY: The Y coordinate into which the element is being dropped
        /// · DroppedID: A long value corresponding to the ID of the element the item has been dropped onto</param>
        /// <returns>Returns True to allow the default behavior to be executed. Return False if you are overriding this behavior.</returns>
        public virtual bool OnPreDropFromTree(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnDropFromTree");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true;
        }

        /// <summary>
        /// OnPreNewAttribute notifies Add-Ins that a new attribute is about to be created on an element. It enables Add-Ins to permit or deny creation of the new attribute.
        /// This event occurs when a user creates a new attribute on an element by either drag-dropping from the Project Browser, using the Attributes Properties dialog, or using the in-place editor on the diagram. The notification is provided immediately before the attribute is created, so that the Add-In can disable addition of the attribute.
        /// Also look at EA_OnPostNewAttribute.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the attribute to be created:
        /// - Type: A string value corresponding to Attribute.Type
        /// - Stereotype: A string value corresponding to Attribute.Stereotype
        /// - ParentID: A long value corresponding to Attribute.ParentID
        /// - ClassifierID: A long value corresponding to Attribute.ClassifierID. </param>
        /// <returns>Return True to enable addition of the new attribute to the model. Return False to disable addition of the new attribute.</returns>
        public virtual bool EA_OnPreNewAttribute(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewAttribute");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// EA_OnPreNewMethod notifies Add-Ins that a new method is about to be created on an element. It enables Add-Ins to permit or deny creation of the new method.
        /// This event occurs when a user creates a new method on an element by either drag-dropping from the Project Browser, using the method Properties dialog, or using the in-place editor on the diagram. The notification is provided immediately before the method is created, so that the Add-In can disable addition of the method.
        /// Also look at OnPostNewMethod.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the method to be created:
        /// - ReturnType: A string value corresponding to Method.ReturnType
        /// - Stereotype: A string value corresponding to Method.Stereotype
        /// - ParentID: A long value corresponding to Method.ParentID
        /// - ClassifierID: A long value corresponding to Method.ClassifierID. </param>
        /// <returns>Return True to enable addition of the new method to the model. Return False to disable addition of the new method.</returns>
        public virtual bool OnPreNewMethod(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewMethod");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// OnPreNewPackage notifies Add-Ins that a new package is about to be created in the model. It enables Add-Ins to permit or deny creation of the new package.
        /// This event occurs when a user drags a new package from the Toolbox or Resources window onto a diagram, or by selecting the New Package icon from the Project Browser. The notification is provided immediately before the package is created, so that the Add-In can disable addition of the package.
        /// Also look at OnPostNewPackage.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the package to be created:
        /// Stereotype: A string value corresponding to Package.Stereotype
        /// ParentID: A long value corresponding to Package.ParentID
        /// DiagramID: A long value corresponding to the ID of the diagram to which the package is being added. </param>
        /// <returns>Return True to enable addition of the new package to the model. Return False to disable addition of the new package.</returns>
        public virtual bool OnPreNewPackage(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewPackage");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        /// <summary>
        /// EA_OnPreNewGlossaryTerm notifies Add-Ins that a new glossary term is about to be created. It enables Add-Ins to permit or deny creation of the new glossary term.
        /// The notification is provided immediately before the glossary term is created, so that the Add-In can disable addition of the element.
        /// Also look at OnPostNewGlossaryTerm.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <param name="Info">Contains the following EventProperty objects for the glossary term to be deleted:
        /// TermID: A long value corresponding to Term.TermID.</param>
        /// <returns>Return True to enable addition of the new glossary term to the model. Return False to disable addition of the new glossary term.</returns>
        public virtual bool OnPreNewGlossaryTerm(EA.Repository repository, EA.EventProperties info)
        {
            _logger.LogInformation("OnPreNewGlossaryTerm");
            _logger.LogInformation(_eaService.GetEventPropertiesString(info));
            return true; 
        }

        private string GetEventPropertiesString(EA.EventProperties eventProperties)
        {
            string eventPropertiesString = string.Empty;

            foreach (EventProperty property in eventProperties)
            {
                eventPropertiesString += "  propertyName = " + property.Name + " propertyValue = " + property.Value + "\n";
            }

            return eventPropertiesString;
        }

        #endregion EA Pre-New Events
    }
}
