// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1401FieldsMustBePrivateBulbItem.cs" company="http://stylecop.codeplex.com">
//   MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. If you cannot locate the  
//   Microsoft Public License, please send an email to dlr@microsoft.com. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
// <summary>
//   The s a 1401 fields must be private bulb item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper513.BulbItems.Maintainability
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Impl;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper513.BulbItems.Framework;
    using StyleCop.ReSharper513.Core;

    #endregion

    /// <summary>
    /// The s a 1401 fields must be private bulb item.
    /// </summary>
    internal class SA1401FieldsMustBePrivateBulbItem : V5BulbItemImpl
    {
        #region Public Methods and Operators

        /// <summary>
        /// The execute transaction inner.
        /// </summary>
        /// <param name="solution">
        /// The solution.
        /// </param>
        /// <param name="textControl">
        /// The text control.
        /// </param>
        public override void ExecuteTransactionInner(ISolution solution, ITextControl textControl)
        {
            IElement element = Utils.GetElementAtCaret(solution, textControl);

            IElement containingElement = (IElement)element.GetContainingElement<IFieldDeclarationNode>(true)
                                         ?? element.GetContainingElement<IMultipleDeclarationNode>(true);

            if (containingElement == null)
            {
                ITreeNode treeNode = (ITreeNode)element;

                containingElement = treeNode.PrevSibling;
            }

            ModifiersUtil.SetAccessRights(containingElement, AccessRights.PRIVATE);
        }

        #endregion
    }
}