using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Sciensoft.Hateoas.Tdd
{
	public class ExpressionCompilationTests
	{
		[Fact(Skip = "Ongoing tests")]
		public void OngoingTestLab()
		{
			// Arrange
			Expression<Func<ResourceModel, string>> expression = r => $"/api/resource/{r.Id}";

			var parameter = expression.Parameters[0];
			var body = expression.Body as MethodCallExpression;

			var argumentType = typeof(ResourceModel);
			var argumentAtRuntime = Activator.CreateInstance(argumentType);

			// Act
			var results = Expression.Lambda(body, parameter).Compile().DynamicInvoke(argumentAtRuntime);

			// Assert
			results.Should().BeNull();
		}

		[Theory]
		[InlineData("F52813B6-346C-11E9-874F-0A27172E52BC")]
		[InlineData("F528191A-346C-11E9-874F-0A27172E52BC")]
		[InlineData("F5281D16-346C-11E9-874F-0A27172E52BC")]
		public void MethodCallExpression_Should_CompileAndReturnResults_ForReflectedTypes(string argumentIdValue)
		{
			// Arrange
			Expression<Func<BookViewModel, string>> expression = r => $"/api/resource/{r.Id}";

			var parameter = expression.Parameters[0];
			var body = expression.Body as MethodCallExpression;

			var sourcePayload = new BookViewModel
			{
				Title = argumentIdValue,
				Id = Guid.Parse(argumentIdValue)
			};

			// Act
			var results = Expression.Lambda(body, parameter).Compile().DynamicInvoke(sourcePayload);

			// Assert
			results.Should().NotBeNull();
			results.ToString().ToLower().Should().Be($"/api/resource/{argumentIdValue.ToLower()}");
		}

		[Fact]
		public void MethodCallExpression_Should_CompileAndReturnResult()
		{
			// Arrange
			var argument = new ResourceModel();
			Expression<Func<ResourceModel, string>> expression = r => $"/api/resource/{r.Id}";

			var parameter = expression.Parameters[0];
			var body = expression.Body as MethodCallExpression;

			// Act
			var results = Expression.Lambda(body, parameter).Compile().DynamicInvoke(argument);

			// Assert
			results.Should().NotBeNull();
			results.ToString().Should().StartWith("/api/resource/");
		}

		[Fact]
		public void ConstantExpression_Should_CompiledAndReturnResult()
		{
			// Arrange
			const string value = "/api/get/{0}";
			var constant = Expression.Constant(value, typeof(string));

			// Act
			var result = Expression.Lambda(constant).Compile().DynamicInvoke();

			// Assert
			result.Should().Be(value);
		}

		[Fact]
		public void ExpressionAsObject_Should_CompiledAndExecuted_ReceivingZeroReturningString()
		{
			// Arrange
			Expression<Func<int, string>> expression = num => $"/api/numbers/{num}";
			var expressionAsObject = expression as Expression;

			var labdaExpression = Expression.Lambda(expressionAsObject, expression.Parameters);
			var @delegate = labdaExpression.Compile();

			var funcType = @delegate.GetType();
			var funcInvoke = funcType.GetMethod("Invoke");
			dynamic funcProjection = funcInvoke.Invoke(@delegate, new object[] { 0 });

			// Act
			var result = funcProjection(0);

			// Assert
			"/api/numbers/0".Should().Be(result);
		}

		[Theory]
		[InlineData("2B9AB500-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9ABB22-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9ABF50-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9AC39C-347E-11E9-91FB-0A27172E52BC")]
		public void ExpressionAsObject_Should_CompiledAndExecuted_ReceivingSampleViewModelAndReturningString(string uuid)
		{
			// Arrange
			var viewModel = new BookViewModel { Id = Guid.Parse(uuid) };
			Expression<Func<BookViewModel, string>> expression = model => $"/api/numbers/{model.Id}";
			var expressionAsObject = expression as Expression;

			var labdaExpression = Expression.Lambda(expressionAsObject, expression.Parameters);
			var @delegate = labdaExpression.Compile();

			var funcType = @delegate.GetType();
			var funcInvoke = funcType.GetMethod("Invoke");
			dynamic funcProjection = funcInvoke.Invoke(@delegate, new object[] { viewModel });

			// Act
			var result = funcProjection(viewModel);

			// Assert
			$"/api/numbers/{uuid}".ToLower().Should().Be(result.ToLower());
		}

		[Theory]
		[InlineData("2B9AB500-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9ABB22-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9ABF50-347E-11E9-91FB-0A27172E52BC")]
		[InlineData("2B9AC39C-347E-11E9-91FB-0A27172E52BC")]
		public void ExpressionAsObject_Should_CompiledAndExecuted_ReceivingObjectAndReturningString(string uuid)
		{
			// Arrange
			var sourcePayload = new { Id = Guid.Parse(uuid) };
			
			Expression<Func<BookViewModel, string>> expression = model => $"/api/numbers/{model.Id}";

			var arguments = (expression.Body as MethodCallExpression).Arguments;

			string constReturn = (arguments.FirstOrDefault(a => a is ConstantExpression) as ConstantExpression).Value.ToString();

			var operandValues = new List<string>();
			foreach (var args in arguments.Where(a => a is UnaryExpression).Cast<UnaryExpression>())
			{
				var operand = args.Operand.ToString();
				var operandMembers = operand.Split('.');
				var operandForBinding = operandMembers.Last();

				operandValues.Add(JObject.FromObject(sourcePayload).GetValue(operandForBinding).ToString());
			}

			// Act
			var result = string.Format(constReturn, operandValues.ToArray());

			// Assert
			$"/api/numbers/{uuid}".ToLower().Should().Be(result.ToLower());
		}

		[Fact]
		public void ExpressionTree_TestOne()
		{
			// Arrange
			var companies = new[]
			{
				"Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light", "Coho Winery", "Wide World Importers",
				"Graphic Design Institute", "Adventure Works", "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
				"Blue Yonder Airlines", "Trey Research", "The Phone Company", "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee"
			}.AsQueryable();

			var expParameter = Expression.Parameter(typeof(string), "company");
			var expArguments = Expression.Constant("ho");
			var expContains = Expression.Call(expParameter, typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }), new[] { expArguments });

			var expContainsIsTrue = Expression.IsTrue(expContains);

			var isTrueCall = Expression.Call(
				typeof(Queryable),
				nameof(Queryable.Where),
				new[] { companies.ElementType },
				companies.Expression,
				Expression.Lambda<Func<string, bool>>(expContainsIsTrue, new[] { expParameter }));

			// Act
			var results = companies.Provider.CreateQuery<string>(isTrueCall);

			// Assert
			results.Any().Should().BeTrue();
			results.Count().Should().BeGreaterThan(0);
		}

		public class ResourceModel
		{
			public Guid Id { get; set; } = Guid.NewGuid();
		}
	}
}
