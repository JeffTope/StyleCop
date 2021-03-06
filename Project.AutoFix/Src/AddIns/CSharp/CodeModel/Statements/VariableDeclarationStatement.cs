//-----------------------------------------------------------------------
// <copyright file="VariableDeclarationStatement.cs">
//     MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------
namespace StyleCop.CSharp.CodeModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A statement declaring a new variable.
    /// </summary>
    /// <subcategory>statement</subcategory>
    public sealed class VariableDeclarationStatement : Statement
    {
        #region Private Fields

        /// <summary>
        /// Indicates whether the item is constant.
        /// </summary>
        private CodeUnitProperty<bool> constant;

        /// <summary>
        /// The inner expression.
        /// </summary>
        private CodeUnitProperty<VariableDeclarationExpression> expression;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the VariableDeclarationStatement class.
        /// </summary>
        /// <param name="proxy">Proxy object for the statement.</param>
        /// <param name="expression">The inner expression.</param>
        /// <param name="constant">Indicates whether the item is constant.</param>
        internal VariableDeclarationStatement(
            CodeUnitProxy proxy, VariableDeclarationExpression expression, bool constant)
            : base(proxy, StatementType.VariableDeclaration)
        {
            Param.AssertNotNull(proxy, "proxy");
            Param.AssertNotNull(expression, "expression");
            Param.Ignore(constant);

            this.expression.Value = expression;
            this.constant.Value = constant;
        }

        /// <summary>
        /// Initializes a new instance of the VariableDeclarationStatement class.
        /// </summary>
        /// <param name="proxy">Proxy object for the statement.</param>
        /// <param name="expression">The inner expression.</param>
        internal VariableDeclarationStatement(CodeUnitProxy proxy, VariableDeclarationExpression expression)
            : base(proxy, StatementType.VariableDeclaration)
        {
            Param.AssertNotNull(proxy, "proxy");
            Param.AssertNotNull(expression, "expression");

            this.expression.Value = expression;
        }

        #endregion Internal Constructors

        #region Public Override Properties

        /// <summary>
        /// Gets the variables defined within this statement.
        /// </summary>
        /// <returns>Returns the collection of variables.</returns>
        public override VariableCollection Variables
        {
            get
            {
                return this.InnerExpression.Variables;
            }
        }

        #endregion Public Override Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the item is constant.
        /// </summary>
        public bool Constant
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.constant.Initialized)
                {
                    Field parentField = this.Parent as Field;
                    if (parentField != null)
                    {
                        this.constant.Value = parentField.Const;
                    }
                    else
                    {
                        this.constant.Value = this.InnerExpression.FindPreviousSibling<ConstToken>() != null;
                    }
                }

                return this.constant.Value;
            }
        }

        /// <summary>
        /// Gets the inner expression for this statement.
        /// </summary>
        public VariableDeclarationExpression InnerExpression
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.expression.Initialized)
                {
                    this.expression.Value = this.FindFirstChild<VariableDeclarationExpression>();
                    if (this.expression.Value == null)
                    {
                        throw new SyntaxException(this.Document, this.LineNumber);
                    }
                }

                return this.expression.Value;
            }
        }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Naming", 
            "CA1721:PropertyNamesShouldNotMatchGetMethods",
            Justification = "API has already been published and should not be changed.")]
        public TypeToken Type
        {
            get
            {
                return this.InnerExpression.Type;
            }
        }

        /// <summary>
        /// Gets the list of declarators for the expression.
        /// </summary>
        public ICollection<VariableDeclaratorExpression> Declarators
        {
            get
            {
                return this.InnerExpression.Declarators;
            }
        }

        #endregion Public Properties

        #region Protected Override Methods

        /// <summary>
        /// Resets the contents of the class.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            this.constant.Reset();
            this.expression.Reset();
        }

        #endregion Protected Override Methods
    }
}
