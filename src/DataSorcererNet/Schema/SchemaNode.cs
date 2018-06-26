using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSorcererNet.Operations;

namespace DataSorcererNet.Schema
{
    public abstract class SchemaNode
    {
        public abstract SchemaNodeKind Kind { get; }

        public static SchemaNode GetSchemaNodeForValue(object value)
        {
            switch (value)
            {
                case int _:
                    return new SchemaNodeInteger();
                case string _:
                    return new SchemaNodeText();
                case decimal _:
                    return new SchemaNodeDecimal();
                case bool _:
                    return new SchemaNodeBoolean();
                case DateTime _:
                    return new SchemaNodeDateTime();
                default:
                    throw new NotSupportedException();
            }
        }
        public static SchemaNodeComplex GetSchemaNodeForElementLiteralOperation(ElementLiteralOperation operation, IEnumerable<SchemaNode> fieldSchemas)
        {
            return new SchemaNodeComplex(operation.Fields.Zip(fieldSchemas, (operationField, schema) => new SchemaNodeComplexField(schema, operationField.Name, operationField.Name, "", true)).ToList());
        }
        public static SchemaNode GetSchemaNodeForUnaryOperation(UnaryOperation operation, SchemaNode schema)
        {
            switch (operation.Operation)
            {
                case QueryOperationNodeType.Not:
                    if (schema.Kind != SchemaNodeKind.Boolean)
                    {
                        throw new InvalidOperationException();
                    }
                    return schema;
                case QueryOperationNodeType.Negate:
                    switch (schema.Kind)
                    {
                        case SchemaNodeKind.Integer:
                            return new SchemaNodeInteger();
                        case SchemaNodeKind.Decimal:
                            return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)schema).ShowAsPercent);
                        case SchemaNodeKind.Currency:
                            return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)schema).Lcid);
                        default:
                            throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
        public static SchemaNode GetSchemaNodeForBinaryOperation(BinaryOperation operation, SchemaNode leftOperandSchema, SchemaNode rightOperantSchema)
        {
            switch (operation.Operation)
            {
                case QueryOperationNodeType.Add:
                case QueryOperationNodeType.Subtract:
                    switch (leftOperandSchema.Kind)
                    {
                        case SchemaNodeKind.Integer:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                    return new SchemaNodeInteger();
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)rightOperantSchema).ShowAsPercent);
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)rightOperantSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Decimal:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                    return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)leftOperandSchema).ShowAsPercent);
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeDecimal(null, null, NullableOr(((SchemaNodeDecimal)leftOperandSchema).ShowAsPercent, ((SchemaNodeDecimal)rightOperantSchema).ShowAsPercent, false));
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)rightOperantSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Currency:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)leftOperandSchema).Lcid);
                                case SchemaNodeKind.Currency:
                                    if (((SchemaNodeCurrency)leftOperandSchema).Lcid.HasValue && ((SchemaNodeCurrency)rightOperantSchema).Lcid.HasValue &&
                                        ((SchemaNodeCurrency)leftOperandSchema).Lcid.Value != ((SchemaNodeCurrency)rightOperantSchema).Lcid.Value)
                                    {
                                        throw new InvalidOperationException();
                                    }
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)rightOperantSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        default:
                            throw new InvalidOperationException();
                    } 
                case QueryOperationNodeType.Divide:
                    switch (leftOperandSchema.Kind)
                    {
                        case SchemaNodeKind.Integer:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                    return new SchemaNodeInteger();
                                case SchemaNodeKind.Decimal:
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeDecimal();
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Decimal:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                case SchemaNodeKind.Decimal:
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)leftOperandSchema).ShowAsPercent);
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Currency:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)leftOperandSchema).Lcid);
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeDecimal();
                                default:
                                    throw new InvalidOperationException();
                            }
                        default:
                            throw new InvalidOperationException();
                    }
                case QueryOperationNodeType.Multiply:
                    switch (leftOperandSchema.Kind)
                    {
                        case SchemaNodeKind.Integer:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                    return new SchemaNodeInteger();
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)rightOperantSchema).ShowAsPercent);
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)rightOperantSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Decimal:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                    return new SchemaNodeDecimal(null, null, ((SchemaNodeDecimal)leftOperandSchema).ShowAsPercent);
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeDecimal(null, null, NullableOr(((SchemaNodeDecimal)leftOperandSchema).ShowAsPercent, ((SchemaNodeDecimal)rightOperantSchema).ShowAsPercent, false));
                                case SchemaNodeKind.Currency:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)rightOperantSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        case SchemaNodeKind.Currency:
                            switch (rightOperantSchema.Kind)
                            {
                                case SchemaNodeKind.Integer:
                                case SchemaNodeKind.Decimal:
                                    return new SchemaNodeCurrency(null, null, ((SchemaNodeCurrency)leftOperandSchema).Lcid);
                                default:
                                    throw new InvalidOperationException();
                            }
                        default:
                            throw new InvalidOperationException();
                    }
                case QueryOperationNodeType.Equal:
                case QueryOperationNodeType.NotEqual:
                    return new SchemaNodeBoolean();
                case QueryOperationNodeType.Greater:
                case QueryOperationNodeType.GreaterOrEqual:
                case QueryOperationNodeType.Less:
                case QueryOperationNodeType.LessOrEqual:
                    if ((leftOperandSchema.Kind == SchemaNodeKind.Integer || leftOperandSchema.Kind == SchemaNodeKind.Decimal || leftOperandSchema.Kind == SchemaNodeKind.Currency) &&
                        (rightOperantSchema.Kind == SchemaNodeKind.Integer || rightOperantSchema.Kind == SchemaNodeKind.Decimal || rightOperantSchema.Kind == SchemaNodeKind.Currency))
                    {
                        return new SchemaNodeBoolean();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                case QueryOperationNodeType.And:
                case QueryOperationNodeType.Or:
                    if (leftOperandSchema.Kind == SchemaNodeKind.Boolean && rightOperantSchema.Kind == SchemaNodeKind.Boolean)
                    {
                        return new SchemaNodeBoolean(((SchemaNodeBoolean)leftOperandSchema).Format == SchemaNodeBooleanFormat.YesNo && ((SchemaNodeBoolean)rightOperantSchema).Format == SchemaNodeBooleanFormat.YesNo ? SchemaNodeBooleanFormat.YesNo : SchemaNodeBooleanFormat.Checkbox);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        private static bool? NullableOr(bool? b1, bool? b2, bool nullValue)
        {
            return !b1.HasValue && !b2.HasValue ? null : (bool?)((b1.HasValue && b1.Value || !b1.HasValue && nullValue) || (b2.HasValue && b2.Value || !b2.HasValue && nullValue));
        }
        private static bool? NullableAnd(bool? b1, bool? b2, bool nullValue)
        {
            return !b1.HasValue && !b2.HasValue ? null : (bool?)((b1.HasValue && b1.Value || !b1.HasValue && nullValue) && (b2.HasValue && b2.Value || !b2.HasValue && nullValue));
        }
    }
    public enum SchemaNodeKind
    {
        Text,
        Choice,
        Boolean,
        DateTime,
        Integer,
        Decimal,
        Currency,
        Complex,
        Collection
    }
}
