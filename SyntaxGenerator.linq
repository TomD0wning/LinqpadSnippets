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
	List<string> args
)
	{
		return LocalDeclarationStatement(VariableDeclaration(IdentifierName(Identifier(TriviaList(), SyntaxKind.VarKeyword, VarKeyword, VarKeyword, TriviaList()))).WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(VariableDeclarator(Identifier(variableName)).WithInitializer(EqualsValueClause(InvocationExpression(IdentifierName(functionName)).AddArgumentListArguments(args.Select(a => Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(a)))).ToArray()))))));
	}

	public static IEnumerable<ArgumentSyntax> GenerateArgList(List<string> args)
	{
		var arguementList = new List<ArgumentSyntax>();
		foreach (var element in args)
		{
			yield return Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(element)));
		}
	}
}

