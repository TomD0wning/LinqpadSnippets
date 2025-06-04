<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.Compilers</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>static Microsoft.CodeAnalysis.CSharp.SyntaxFactory</Namespace>
</Query>

void Main()
{
	//var args = new List<string>() { "arg1", "argh2", "arg3" };
	//var func = SyntaxGenerator.GenerateFunctionAssignment("function", "variable", args);
	//func.NormalizeWhitespace().ToFullString().Dump();

	var x = new int[3] {1,2,3};
	var y = new int[x.Length + 1];
	x.CopyTo(y, 0);
	y[y.Length -1] = 6;
	y[y.Length -1].Dump();
}


public static class SyntaxGenerator
{
	private const string VarKeyword = "var";

	public static StatementSyntax GenerateFunctionAssignment(
		string functionName,
		string variableName,
		params object[] arguments
	)
	{
		return LocalDeclarationStatement(
				VariableDeclaration(
						IdentifierName(
							Identifier(
								TriviaList(),
								SyntaxKind.VarKeyword,
								VarKeyword,
								VarKeyword,
								TriviaList()
							)
						)
					)
					.WithVariables(
						SingletonSeparatedList<VariableDeclaratorSyntax>(
							VariableDeclarator(Identifier(variableName))
								.WithInitializer(
									EqualsValueClause(
										InvocationExpression(IdentifierName(functionName))
											.AddArgumentListArguments(
												arguments
													?.Select(a =>
														Argument(GenerateLiteralExpression(a))
													)
													.ToArray() ?? []
											)
									)
								)
						)
					)
			)
			.NormalizeWhitespace();
	}

	public static StatementSyntax GenerateVariableAssignment(
		string leftHandVariableName,
		string RightHandVariableName
	)
	{
		return LocalDeclarationStatement(
				VariableDeclaration(
						IdentifierName(
							Identifier(
								TriviaList(),
								SyntaxKind.VarKeyword,
								VarKeyword,
								VarKeyword,
								TriviaList()
							)
						)
					)
					.WithVariables(
						SingletonSeparatedList<VariableDeclaratorSyntax>(
							VariableDeclarator(Identifier(leftHandVariableName))
								.WithInitializer(
									EqualsValueClause(IdentifierName(RightHandVariableName))
								)
						)
					)
			)
			.NormalizeWhitespace();
	}

	public static StatementSyntax GenerateLiteralAssignment(
		string variableName,
		object literalValue
	)
	{
		return LocalDeclarationStatement(
				VariableDeclaration(
						IdentifierName(
							Identifier(
								TriviaList(),
								SyntaxKind.VarKeyword,
								VarKeyword,
								VarKeyword,
								TriviaList()
							)
						)
					)
					.WithVariables(
						SingletonSeparatedList<VariableDeclaratorSyntax>(
							VariableDeclarator(Identifier(variableName))
								.WithInitializer(
									EqualsValueClause(GenerateLiteralExpression(literalValue))
								)
						)
					)
			)
			.NormalizeWhitespace();
	}

	public static StatementSyntax GenerateMathematicalVariableExpression(
		SyntaxKind operationType,
		string leftHandArgument,
		string rightHandArgument
	)
	{
		return ExpressionStatement(
				BinaryExpression(
					operationType,
					IdentifierName(leftHandArgument),
					IdentifierName(rightHandArgument)
				)
			)
			.NormalizeWhitespace();
	}

	public static StatementSyntax GenerateMathematicalExpression(
		Formula formula,
		SyntaxKind operationType,
		string variableName
	)
	{
		var expression = BuildExpression(formula, operationType);
		return LocalDeclarationStatement(
				VariableDeclaration(IdentifierName(VarKeyword))
					.AddVariables(
						VariableDeclarator(Identifier(variableName))
							.WithInitializer(EqualsValueClause(expression))
					)
			)
			.NormalizeWhitespace();
	}

	private static LiteralExpressionSyntax GenerateLiteralExpression(object o)
	{
		return o switch
		{
			int i => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)),
			bool b
				=> b
					? LiteralExpression(
						SyntaxKind.TrueLiteralExpression,
						Literal((int)SyntaxKind.TrueKeyword)
					)
					: LiteralExpression(
						SyntaxKind.FalseLiteralExpression,
						Literal((int)SyntaxKind.FalseKeyword)
					),
			string s => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(s)),
			double d => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(d)),
			decimal d => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(d)),
			null => LiteralExpression(SyntaxKind.NullLiteralExpression),
			_ => throw new ArgumentException("Unsupported type"),
		};
	}

	private static ExpressionSyntax BuildExpression(Formula formula, SyntaxKind operationType)
	{
		return formula switch
		{
			VariableFormula value => IdentifierName(value.Name),
			CalculationFormula calculation
				=> BinaryExpression(
					operationType,
					BuildExpression(calculation.Left, operationType),
					BuildExpression(calculation.Right, operationType)
				),
			_ => throw new ArgumentException("Unsupported formula type."),
		};
	}
}

public abstract class Formula { }

public abstract class InfixFormula : Formula
{
	public Formula Left { get; set; }
	public Formula Right { get; set; }

	public InfixFormula(Formula left, Formula right) => (Left, Right) = (left, right);
}

public abstract class CalculationFormula : InfixFormula
{
	public CalculationFormula(Formula left, Formula right)
		: base(left, right) { }
}

public class VariableFormula : Formula
{
	public string Name { get; set; }

	public VariableFormula(string name) => Name = name;
}



