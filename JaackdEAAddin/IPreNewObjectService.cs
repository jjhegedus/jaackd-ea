using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin
{
    internal interface IPreNewObjectService
    {


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
        public bool OnPreNewElement(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewConnector(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewDiagram(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewDiagramObject(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreDropFromTree(EA.Repository Repository, EA.EventProperties Info);

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
        public bool EA_OnPreNewAttribute(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewMethod(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewPackage(EA.Repository Repository, EA.EventProperties Info);

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
        public bool OnPreNewGlossaryTerm(EA.Repository Repository, EA.EventProperties Info);

        #endregion EA Pre-New Events
    }
}
