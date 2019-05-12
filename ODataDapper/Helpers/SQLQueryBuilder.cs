using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Query;
using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData.Query;

namespace ODataDapper.Helpers
{
    /// <summary>
    /// The SQL queries builder helper class.
    /// </summary>
    public class SQLQueryBuilder
    {
        #region Properties
        private readonly ODataQueryOptions _queryOptions;
        private readonly EdmEntityType _edmEntityType;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLQueryBuilder"/> class.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        public SQLQueryBuilder(ODataQueryOptions queryOptions)
        {
            _queryOptions = queryOptions;
            _edmEntityType = queryOptions.Context.ElementType as EdmEntityType;
        }
        #endregion

        #region SQL_PRIVATE_METHODS
        /// <summary>
        /// Builds the single filter clause.
        /// </summary>
        /// <param name="propertyNode">The property node.</param>
        /// <param name="valueNode">The value node.</param>
        /// <param name="operatorKind">Kind of the operator.</param>
        /// <returns>
        /// Returns built single filter clause.
        /// </returns>
        private string BuildSingleClause(SingleValuePropertyAccessNode propertyNode, ConstantNode valueNode, BinaryOperatorKind operatorKind)
        {
            string operatorString = string.Empty;

            switch (operatorKind)
            {
                case BinaryOperatorKind.Equal:
                    operatorString = "=";
                    break;

                case BinaryOperatorKind.NotEqual:
                    operatorString = "!=";
                    break;

                case BinaryOperatorKind.GreaterThan:
                    operatorString = ">";
                    break;

                case BinaryOperatorKind.GreaterThanOrEqual:
                    operatorString = ">=";
                    break;

                case BinaryOperatorKind.LessThan:
                    operatorString = "<";
                    break;

                case BinaryOperatorKind.LessThanOrEqual:
                    operatorString = "<=";
                    break;
            }

            string valueString = valueNode.Value?.ToString() ?? "null";

            if (valueNode.Value == null)
            {
                if (operatorKind == BinaryOperatorKind.Equal)
                    return $"({propertyNode.Property.Name} IS NULL)";

                if (operatorKind == BinaryOperatorKind.NotEqual)
                    return $"({propertyNode.Property.Name} IS NOT NULL)";
            }

            return $"({propertyNode.Property.Name} {operatorString} {valueString})";
        }

        /// <summary>
        /// Builds from property node - meaning builds the clause from element property, e.g. Name of the person.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="operatorKind">Kind of the operator.</param>
        /// <returns>
        /// Returns built clause from property node.
        /// </returns>
        private string BuildFromPropertyNode(SingleValuePropertyAccessNode left, SingleValueNode right, BinaryOperatorKind operatorKind)
        {
            string result = string.Empty;
            if (right is ConstantNode)
            {
                result = BuildSingleClause(left, right as ConstantNode, operatorKind);
            }

            var rightSource = (right as ConvertNode)?.Source;
            if (rightSource is ConstantNode)
            {
                result = BuildSingleClause(left, rightSource as ConstantNode, operatorKind);
            }

            return result;
        }

        /// <summary>
        /// Builds the where clause from the property node - checks with recursion first.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// Returns built where clause.
        /// </returns>
        private string BuildWhereClause(SingleValueNode node)
        {
            string result = string.Empty;

            var operatorNode = node as BinaryOperatorNode;
            if (operatorNode == null)
                return result;

            var left = operatorNode.Left;
            var right = operatorNode.Right;

            if (left is SingleValuePropertyAccessNode)
                return BuildFromPropertyNode(left as SingleValuePropertyAccessNode, right, operatorNode.OperatorKind);

            if (left is ConvertNode)
            {
                var leftSource = ((ConvertNode)left).Source;

                if (leftSource is SingleValuePropertyAccessNode)
                    return BuildFromPropertyNode(leftSource as SingleValuePropertyAccessNode, right, operatorNode.OperatorKind);

                result += BuildWhereClause(leftSource);
            }

            if (left is BinaryOperatorNode)
            {
                result += BuildWhereClause(left);
            }

            result += " " + operatorNode.OperatorKind;

            if (right is BinaryOperatorNode)
            {
                result += " " + BuildWhereClause(right);
            }

            return result;
        }

        /// <summary>
        /// Builds the where clause.
        /// </summary>
        /// <param name="filterQueryOption">The filter query option.</param>
        /// <returns>
        /// Returns built where clause.
        /// </returns>
        private string BuildWhereClause(FilterQueryOption filterQueryOption)
        {
            if (filterQueryOption == null)
                return string.Empty;

            var whereClause = BuildWhereClause(filterQueryOption.FilterClause.Expression);
            return whereClause;
        }

        /// <summary>
        /// Determines whether the entity [has declare key].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [has declare key]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasDeclareKey()
        {
            if (_edmEntityType.DeclaredKey == null)
                return false;

            return _edmEntityType.DeclaredKey.Any();
        }

        /// <summary>
        /// Builds the order clause.
        /// </summary>
        /// <param name="orderByQueryOption">The order by query option.</param>
        /// <returns>
        /// Returns built order clause.
        /// </returns>
        private string BuildOrderClause(OrderByQueryOption orderByQueryOption)
        {
            var orderClause = string.Empty;

            if (orderByQueryOption != null)
            {
                var columns = orderByQueryOption.RawValue.Split(',').AsEnumerable();
                orderClause = string.Join(",", columns);
            }
            else
            {
                var hasDeclareKey = HasDeclareKey();
                if (hasDeclareKey)
                {
                    var keys = _edmEntityType.DeclaredKey;
                    orderClause = string.Join(",", keys.Select(k => k.Name));
                }
                else
                {
                    var firstProperty = _edmEntityType.DeclaredProperties.FirstOrDefault();
                    if (firstProperty != null)
                        orderClause = firstProperty.Name;
                }
            }

            return orderClause;
        }

        /// <summary>
        /// Builds the skip top clause.
        /// </summary>
        /// <param name="topQueryOption">The top query option.</param>
        /// <param name="skipQueryOption">The skip query option.</param>
        /// <returns>
        /// Returns built top clause.
        /// </returns>
        private string BuildSkipTopClause(TopQueryOption topQueryOption, SkipQueryOption skipQueryOption)
        {
            string skipTopClause = string.Empty;
            if (topQueryOption != null)
            {
                int skipValue = skipQueryOption?.Value ?? 0;
                skipTopClause = $"OFFSET {skipValue} ROWS FETCH NEXT {topQueryOption.Value} ROWS ONLY";
            }

            if (topQueryOption == null && skipQueryOption != null)
            {
                skipTopClause = $"OFFSET {skipQueryOption.Value} ROWS";
            }

            return skipTopClause;
        }

        /// <summary>
        /// Builds the order clause that skips and takes a number of elements.
        /// </summary>
        /// <param name="orderByQueryOption">The order by query option.</param>
        /// <param name="topQueryOption">The top query option.</param>
        /// <param name="skipQueryOption">The skip query option.</param>
        /// <returns>
        /// Returns sorted and filtered order clause.
        /// </returns>
        private string BuildOrderClause(OrderByQueryOption orderByQueryOption, TopQueryOption topQueryOption, SkipQueryOption skipQueryOption)
        {
            string orderClause = BuildOrderClause(orderByQueryOption);
            string skipTopClause = BuildSkipTopClause(topQueryOption, skipQueryOption);

            if (string.IsNullOrEmpty(skipTopClause))
                return orderClause;

            return $"{orderClause} {skipTopClause}";
        }
        #endregion

        #region SQL_PUBLIC_METHODS
        /// <summary>
        /// Translates the query options to the SQL.
        /// </summary>
        /// <returns>
        /// Returns the query options translated to SQL.
        /// </returns>
        public string ToSql()
        {
            string sql = "";
            string whereClause = BuildWhereClause(_queryOptions.Filter);
            if (!string.IsNullOrEmpty(whereClause))
            {
                sql = $"{sql} WHERE {whereClause}";
            }

            string orderClause = BuildOrderClause(_queryOptions.OrderBy, _queryOptions.Top, _queryOptions.Skip);
            if (!string.IsNullOrEmpty(orderClause))
            {
                sql = $"{sql} ORDER BY {orderClause}";
            }

            return sql;
        }

        /// <summary>
        /// Translates to count sql query.
        /// </summary>
        /// <returns>
        /// Returns query options translated to count sql query
        /// </returns>
        public KeyValuePair<string, string> ToCountSql()
        {
            string sql = $@"SELECT COUNT(*) FROM ";

            string whereClause = BuildWhereClause(_queryOptions.Filter);
            string compositeWhereClause = "";
            if (!string.IsNullOrEmpty(whereClause))
            {
                compositeWhereClause = $" WHERE {whereClause}";
            }

            return new KeyValuePair<string, string>(sql, compositeWhereClause);
        }
        #endregion
    }
}