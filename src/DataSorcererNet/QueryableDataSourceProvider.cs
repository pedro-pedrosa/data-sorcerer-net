using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataSorcererNet.Operations;
using DataSorcererNet.Schema;

namespace DataSorcererNet 
{
    public class QueryableDataSourceProvider : IDataSourceProvider
    {
        public QueryableDataSourceProvider(ObjectElementService objectService, IQueryable queryable, SchemaNode schema) 
        {
            _objectService = objectService;
            _queryable = queryable;
            _schema = schema;
        }
        protected ObjectElementService _objectService;
        protected IQueryable _queryable;
        protected SchemaNode _schema;
        protected Stack<FunctionScope> _scopeStack;
        protected Dictionary<SchemaNodeComplex, ObjectElementPropertyResolver> _schemaTypeMappings;

        object IDataSourceProvider.Execute(QueryOperation query)
        {
            _scopeStack = new Stack<FunctionScope>();
            var result = Visit(query);
            switch (result.ResultSchema.Kind)
            {
                case SchemaNodeKind.Collection:
                    return ExecuteCollection(result.Expression);
                default:
                    throw new NotImplementedException();
            }
        }
        protected IEnumerable<ObjectElement> ExecuteCollection(Expression expression)
        {
            var elementSchemaType = (SchemaNodeComplex)((SchemaNodeCollection)_schema).ElementSchema;
            _objectService.GetTypeForElement(elementSchemaType, out var namesResolver);
            foreach (object value in _queryable.Provider.CreateQuery(expression))
            {
                yield return new ObjectElement(value, elementSchemaType, namesResolver);
            }
        }

        protected VisitResult Visit(QueryOperation operation)
        {
            switch (operation.Operation)
            {
                case QueryOperationNodeType.Parameter:
                    return VisitParameter((ParameterOperation)operation);
                case QueryOperationNodeType.DataSourceReference:
                    return VisitDataSourceReference((DataSourceReferenceOperation)operation);
                case QueryOperationNodeType.Literal:
                    return VisitLiteral((LiteralOperation)operation);
                case QueryOperationNodeType.Count:
                    return VisitCount((CountOperation)operation);
                case QueryOperationNodeType.Filter:
                    return VisitFilter((FilterOperation)operation);
                case QueryOperationNodeType.Projection:
                    return VisitProjection((ProjectionOperation)operation);
                case QueryOperationNodeType.Sort:
                    return VisitSort((SortOperation)operation);
                case QueryOperationNodeType.TakePage:
                    return VisitTakePage((TakePageOperation)operation);
                case QueryOperationNodeType.ElementLiteral:
                    return VisitElementLiteral((ElementLiteralOperation)operation);
                case QueryOperationNodeType.FieldReference:
                    return VisitFieldReference((FieldReferenceOperation)operation);
                case QueryOperationNodeType.Add:
                    return VisitAdd((AddOperation)operation);
                case QueryOperationNodeType.Subtract:
                    return VisitSubtract((SubtractOperation)operation);
                case QueryOperationNodeType.Divide:
                    return VisitDivide((DivideOperation)operation);
                case QueryOperationNodeType.Multiply:
                    return VisitMultiply((MultiplyOperation)operation);
                case QueryOperationNodeType.Negate:
                    return VisitNegate((NegateOperation)operation);
                case QueryOperationNodeType.Equal:
                    return VisitEqual((EqualOperation)operation);
                case QueryOperationNodeType.NotEqual:
                    return VisitNotEqual((NotEqualOperation)operation);
                case QueryOperationNodeType.Greater:
                    return VisitGreater((GreaterOperation)operation);
                case QueryOperationNodeType.GreaterOrEqual:
                    return VisitGreaterOrEqual((GreaterOrEqualOperation)operation);
                case QueryOperationNodeType.Less:
                    return VisitLess((LessOperation)operation);
                case QueryOperationNodeType.LessOrEqual:
                    return VisitLessOrEqual((LessOrEqualOperation)operation);
                case QueryOperationNodeType.And:
                    return VisitAnd((AndOperation)operation);
                case QueryOperationNodeType.Or:
                    return VisitOr((OrOperation)operation);
                case QueryOperationNodeType.Not:
                    return VisitNot((NotOperation)operation);
                case QueryOperationNodeType.If:
                    return VisitIf((IfOperation)operation);
                case QueryOperationNodeType.Contains:
                    return VisitContains((ContainsOperation)operation);
                case QueryOperationNodeType.StartsWith:
                    return VisitStartsWith((StartsWithOperation)operation);
                case QueryOperationNodeType.EndsWith:
                    return VisitEndsWith((EndsWithOperation)operation);
                case QueryOperationNodeType.ToUpperCase:
                    return VisitToUpperCase((ToUpperCaseOperation)operation);
                case QueryOperationNodeType.ToLowerCase:
                    return VisitToLowerCase((ToLowerCaseOperation)operation);
            }
            throw new Exception("Unknown query expression type");
        }

        protected VisitResult VisitParameter(ParameterOperation operation)
        {
            var variable = GetScopedVariable(operation.Name);
            return new VisitResult 
            {
                ResultSchema = variable.Schema,
                Expression = variable.Expression,
            };
        }
        protected VisitResult VisitDataSourceReference(DataSourceReferenceOperation operation)
        {
            return new VisitResult 
            {
                ResultSchema = _schema,
                Expression = _queryable.Expression,
                ElementType = _queryable.ElementType,
            };
        }
        protected VisitResult VisitLiteral(LiteralOperation operation)
        {
            return new VisitResult 
            {
                ResultSchema = SchemaNode.GetSchemaNodeForValue(operation.Value),
                Expression = Expression.Constant(operation.Value),
            };
        }
        
        protected VisitResult VisitCount(CountOperation operation)
        {
            var source = Visit(operation.Source);
            if (source.ResultSchema.Kind != SchemaNodeKind.Collection)
            {
                throw new InvalidOperationException("Can only use call operations on collections");
            }
            var arguments = new[] { source.Expression };
            return new VisitResult 
            {
                ResultSchema = new SchemaNodeInteger(null, 0),
                Expression = Expression.Call(
                    null,
                    typeof(Queryable).GetMethod("Count", arguments.Select(expr => expr.Type).ToArray()),
                    arguments),
            };
        }
        protected VisitResult VisitFilter(FilterOperation operation)
        {
            var source = Visit(operation.Source);
            if (source.ResultSchema.Kind != SchemaNodeKind.Collection)
            {
                throw new InvalidOperationException("Can only use filter operations on collections");
            }
            var parameter = Expression.Parameter(source.ElementType, operation.ParameterName);
            using (CreateScope(new[] { new FunctionScopeVariable(operation.ParameterName, ((SchemaNodeCollection)source.ResultSchema).ElementSchema, parameter) }))
            {
                var predicate = Visit(operation.Predicate);
                if (source.ResultSchema.Kind != SchemaNodeKind.Boolean)
                {
                    throw new InvalidOperationException("The predicate of a filter operation must be a boolean operation");
                }
                var lambda = Expression.Lambda(predicate.Expression, new[] { parameter });
                var arguments = new[] { source.Expression, Expression.Quote(lambda) };
                return new VisitResult 
                {
                    ResultSchema = source.ResultSchema,
                    Expression = Expression.Call(
                        null,
                        typeof(Queryable).GetMethod("Where", arguments.Select(expr => expr.Type).ToArray()),
                        arguments),
                    ElementType = source.ElementType,
                };
            }
        }
        protected VisitResult VisitProjection(ProjectionOperation operation)
        {
            var source = Visit(operation.Source);
            if (source.ResultSchema.Kind != SchemaNodeKind.Collection)
            {
                throw new InvalidOperationException("Can only use projection operations on collections");
            }
            var parameter = Expression.Parameter(source.ElementType, operation.ParameterName);
            using (CreateScope(new[] { new FunctionScopeVariable(operation.ParameterName, ((SchemaNodeCollection)source.ResultSchema).ElementSchema, parameter) }))
            {
                var projection = Visit(operation.Projection);
                var lambda = Expression.Lambda(projection.Expression, new[] { parameter });
                var arguments = new[] { source.Expression, Expression.Quote(lambda) };
                return new VisitResult 
                {
                    ResultSchema = new SchemaNodeCollection(projection.ResultSchema),
                    Expression = Expression.Call(
                        null,
                        typeof(Queryable).GetMethod("Select", arguments.Select(expr => expr.Type).ToArray()),
                        arguments),
                    ElementType = projection.Expression.Type,
                };
            }
        }
        protected VisitResult VisitSort(SortOperation operation)
        {
            var source = Visit(operation.Source);
            if (source.ResultSchema.Kind != SchemaNodeKind.Collection)
            {
                throw new InvalidOperationException("Can only use sort operations on collections");
            }
            var result = source.Expression;
            foreach (var step in operation.Steps)
            {
                var parameter = Expression.Parameter(source.ElementType, operation.ParameterName);
                using (CreateScope(new[] { new FunctionScopeVariable(operation.ParameterName, ((SchemaNodeCollection)source.ResultSchema).ElementSchema, parameter) }))
                {
                    var sort = Visit(step.SortBy);
                    var lambda = Expression.Lambda(sort.Expression, new[] { parameter });
                    var arguments = new[] { result, Expression.Quote(lambda) };
                    var method = result == source.Expression ?
                        (step.Ascending ? "OrderBy" : "OrderByDescending") :
                        (step.Ascending ? "ThenBy" : "ThenByDescending");
                    result = Expression.Call(
                        null,
                        typeof(Queryable).GetMethod(method, arguments.Select(expr => expr.Type).ToArray()),
                        arguments);
                }
            }
            return new VisitResult 
            {
                ResultSchema = source.ResultSchema,
                Expression = result,
                ElementType = source.ElementType,
            };
        }
        protected VisitResult VisitTakePage(TakePageOperation operation)
        {
            var source = Visit(operation.Source);
            if (source.ResultSchema.Kind != SchemaNodeKind.Collection)
            {
                throw new InvalidOperationException("Can only use take page operations on collections");
            }
            var skipArguments = new[] { source.Expression, Expression.Constant(operation.Start) };
            var skipCall = Expression.Call(
                null,
                typeof(Queryable).GetMethod("Skip", skipArguments.Select(expr => expr.Type).ToArray()),
                skipArguments);
            var takeArguments = new Expression[] { skipCall, Expression.Constant(operation.Count) };
            return new VisitResult 
            {
                ResultSchema = source.ResultSchema,
                Expression = Expression.Call(
                    null,
                    typeof(Queryable).GetMethod("Take", takeArguments.Select(expr => expr.Type).ToArray()),
                    takeArguments),
                ElementType = source.ElementType,
            };
        }
        
        protected VisitResult VisitElementLiteral(ElementLiteralOperation operation)
        {
            var fields = operation.Fields.Select(field => Visit(field.Value).ResultSchema).ToList();
            var schema = SchemaNode.GetSchemaNodeForElementLiteralOperation(operation, fields);
            Type objectType = _objectService.GetTypeForElement(schema, out var mappings);
            _schemaTypeMappings[schema] = mappings;
            return new VisitResult
            {
                ResultSchema = schema,
                Expression = Expression.MemberInit(
                    Expression.New(objectType.GetConstructor(new Type[0])),
                    operation.Fields.Select(field => Expression.Bind(objectType.GetProperty(mappings.Get(field.Name)), Visit(field.Value).Expression))),
            };
        }
        protected VisitResult VisitFieldReference(FieldReferenceOperation operation)
        {
            var element = Visit(operation.Element);
            SchemaNode resultSchema;
            switch (element.ResultSchema.Kind)
            {
                case SchemaNodeKind.Complex:
                    var field = ((SchemaNodeComplex)element.ResultSchema).Fields.FirstOrDefault(f => f.Name == operation.FieldName);
                    if (field == null)
                    {
                        throw new InvalidOperationException($"Invalid field name '{operation.FieldName}'");
                    }
                    resultSchema = field.Schema;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return new VisitResult
            {
                ResultSchema = resultSchema,
                Expression = Expression.MakeMemberAccess(
                    element.Expression,
                    element.Expression.Type.GetProperty(_schemaTypeMappings[(SchemaNodeComplex)element.ResultSchema].Get(operation.FieldName))),
            };
        }

        protected VisitResult VisitUnary(Func<Expression, Expression> factoryMethod, UnaryOperation operation)
        {
            var operand = Visit(operation.Operand);
            return new VisitResult
            {
                ResultSchema = SchemaNode.GetSchemaNodeForUnaryOperation(operation, operand.ResultSchema),
                Expression = factoryMethod(operand.Expression),
            };
        }
        protected VisitResult VisitBinary(Func<Expression, Expression, Expression> factoryMethod, BinaryOperation operation)
        {
            var leftOperand = Visit(operation.LeftOperand);
            var rightOperand = Visit(operation.RightOperand);
            return new VisitResult
            {
                ResultSchema = SchemaNode.GetSchemaNodeForBinaryOperation(operation, leftOperand.ResultSchema, rightOperand.ResultSchema),
                Expression = factoryMethod(leftOperand.Expression, rightOperand.Expression),
            };
        }
        protected VisitResult VisitAdd(AddOperation operation)
        {
            return VisitBinary(Expression.Add, operation);
        }
        protected VisitResult VisitSubtract(SubtractOperation operation)
        {
            return VisitBinary(Expression.Subtract, operation);
        }
        protected VisitResult VisitDivide(DivideOperation operation)
        {
            return VisitBinary(Expression.Divide, operation);
        }
        protected VisitResult VisitMultiply(MultiplyOperation operation)
        {
            return VisitBinary(Expression.Multiply, operation);
        }
        protected VisitResult VisitNegate(NegateOperation operation)
        {
            return VisitUnary(Expression.Negate, operation);
        }

        protected VisitResult VisitEqual(EqualOperation operation)
        {
            return VisitBinary(Expression.Equal, operation);
        }
        protected VisitResult VisitNotEqual(NotEqualOperation operation)
        {
            return VisitBinary(Expression.NotEqual, operation);
        }
        protected VisitResult VisitGreater(GreaterOperation operation)
        {
            return VisitBinary(Expression.GreaterThan, operation);
        }
        protected VisitResult VisitGreaterOrEqual(GreaterOrEqualOperation operation)
        {
            return VisitBinary(Expression.GreaterThanOrEqual, operation);
        }
        protected VisitResult VisitLess(LessOperation operation)
        {
            return VisitBinary(Expression.LessThan, operation);
        }
        protected VisitResult VisitLessOrEqual(LessOrEqualOperation operation)
        {
            return VisitBinary(Expression.LessThanOrEqual, operation);
        }
        protected VisitResult VisitAnd(AndOperation operation)
        {
            return VisitBinary(Expression.And, operation);
        }
        protected VisitResult VisitOr(OrOperation operation)
        {
            return VisitBinary(Expression.Or, operation);
        }
        protected VisitResult VisitNot(NotOperation operation)
        {
            return VisitUnary(Expression.Not, operation);
        }
        protected VisitResult VisitIf(IfOperation operation)
        {
            var condition = Visit(operation.Condition);
            if (condition.ResultSchema.Kind != SchemaNodeKind.Boolean)
            {
                throw new InvalidOperationException("The condition in an if operation must be a boolean");
            }
            var trueOperation = Visit(operation.TrueOperation);
            var falseOperation = Visit(operation.FalseOperation);
            if (trueOperation.ResultSchema.Kind != falseOperation.ResultSchema.Kind)
            {
                throw new InvalidOperationException("The result cases in a condition must be of the same type");
            }
            return new VisitResult
            {
                ResultSchema = trueOperation.ResultSchema,
                Expression = Expression.Condition(
                    condition.Expression,
                    trueOperation.Expression,
                    falseOperation.Expression),
            };
        }

        protected VisitResult VisitMethodCall(QueryOperation callee, string methodName, QueryOperation[] arguments, SchemaNodeKind sourceType, SchemaNodeKind[] argumentTypes, SchemaNode resultSchema)
        {
            var visitedCallee = Visit(callee);
            if (visitedCallee.ResultSchema.Kind != sourceType)
            {
                throw new InvalidOperationException();
            }
            var visitedArguments = arguments.Select(arg => Visit(arg)).ToArray();
            if (!visitedArguments.Zip(argumentTypes, (arg, type) => arg.ResultSchema.Kind == type).All(e => e))
            {
                throw new InvalidOperationException();
            }
            return new VisitResult
            {
                ResultSchema = resultSchema,
                Expression = Expression.Call(
                    visitedCallee.Expression,
                    visitedCallee.Expression.Type.GetMethod(methodName, visitedArguments.Select(arg => arg.Expression.Type).ToArray()),
                    visitedArguments.Select(arg => arg.Expression).ToArray()),
            };
        }
        protected VisitResult VisitContains(ContainsOperation operation)
        {
            return VisitMethodCall(operation.Source, "Contains", new[] { operation.Search }, SchemaNodeKind.Text, new[] { SchemaNodeKind.Text }, new SchemaNodeBoolean());
        }
        protected VisitResult VisitStartsWith(StartsWithOperation operation)
        {
            return VisitMethodCall(operation.Source, "StartsWith", new[] { operation.Fragment }, SchemaNodeKind.Text, new[] { SchemaNodeKind.Text }, new SchemaNodeBoolean());
        }
        protected VisitResult VisitEndsWith(EndsWithOperation operation)
        {
            return VisitMethodCall(operation.Source, "EndsWith", new[] { operation.Fragment }, SchemaNodeKind.Text, new[] { SchemaNodeKind.Text }, new SchemaNodeBoolean());
        }
        protected VisitResult VisitToUpperCase(ToUpperCaseOperation operation)
        {
            return VisitMethodCall(operation.Text, "ToUpper", new QueryOperation[] { }, SchemaNodeKind.Text, new SchemaNodeKind[] { }, new SchemaNodeText());
        }
        protected VisitResult VisitToLowerCase(ToLowerCaseOperation operation)
        {
            return VisitMethodCall(operation.Text, "ToLower", new QueryOperation[] { }, SchemaNodeKind.Text, new SchemaNodeKind[] { }, new SchemaNodeText());
        }

        protected class FunctionScope : IDisposable
        {
            private Stack<FunctionScope> _scopeStack;
            public FunctionScope(Stack<FunctionScope> scopeStack, IEnumerable<FunctionScopeVariable> variables)
            {
                _scopeStack = scopeStack;
                Variables = variables.ToDictionary(v => v.Name);
            }
            public Dictionary<string, FunctionScopeVariable> Variables { get; private set; }

            public void Dispose()
            {
                if (_scopeStack.Any() && _scopeStack.Peek() == this)
                {
                    _scopeStack.Pop();
                }
            }
        }
        protected class FunctionScopeVariable
        {
            public FunctionScopeVariable(string name, SchemaNode schema, Expression expression)
            {
                Name = name;
                Schema = schema;
                Expression = expression;
            }
            public string Name { get; private set; }
            public SchemaNode Schema { get; private set; }
            public Expression Expression { get; private set; }
        }
        protected FunctionScope CreateScope(IEnumerable<FunctionScopeVariable> variables)
        {
            var scope = new FunctionScope(_scopeStack, variables);
            _scopeStack.Push(scope);
            return scope;
        }
        protected FunctionScopeVariable GetScopedVariable(string name)
        {
            foreach (var scope in _scopeStack)
            {
                if (scope.Variables.ContainsKey(name))
                {
                    return scope.Variables[name];
                }
            }
            return null;
        }

        protected class VisitResult {
            public SchemaNode ResultSchema { get; set; }
            public Expression Expression { get; set; }
            public Type ElementType { get; set; }
        }
    }
}