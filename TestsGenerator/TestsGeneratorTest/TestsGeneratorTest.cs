using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsGenerator;

namespace TestsGeneratorTest
{
    [TestClass]
    public class TestsGeneratorTest
    {
        private TestClassInfo[] _result;

        [TestInitialize]
        public void Setup()
        {
            string fileContent;
            using (var fStream = new StreamReader(@"ListGenerator.cs"))
            {
                fileContent = fStream.ReadToEnd();
            }

            _result = TestGenerator.Generate(fileContent);
        }

        [TestMethod]
        public void TestResultCounts()
        {
            Assert.AreEqual(_result.Length, 1);
        }

        [TestMethod]
        public void TestUsing()
        {
            var usingNames = CSharpSyntaxTree.ParseText(_result[0].TestClassCode).GetRoot().DescendantNodes()
                .OfType<UsingDirectiveSyntax>().Select(item => item.Name.ToString()).ToArray();

            Assert.AreEqual(usingNames.Length, 5);

            string[] expectedUsing =
            {
                "System", "System.Collections", "System.Collections.Generic", "FakerLibrary",
                "Microsoft.VisualStudio.TestTools.UnitTesting"
            };

            for (var i = 0; i < expectedUsing.Length; i++)
                Assert.AreEqual(expectedUsing[i], usingNames[i]);
        }

        [TestMethod]
        public void TestNamespaces()
        {
            var namespaces = CSharpSyntaxTree.ParseText(_result[0].TestClassCode).GetRoot().DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>().Select(item => item.Name.ToString()).ToArray();

            Assert.AreEqual(namespaces.Length, 1);
            Assert.AreEqual(namespaces[0], "ListGeneratorUnitTests");
        }


        [TestMethod]
        public void TestClassName()
        {
            var classes = CSharpSyntaxTree.ParseText(_result[0].TestClassCode).GetRoot().DescendantNodes()
                .OfType<ClassDeclarationSyntax>().Select(item => item.Identifier.ValueText).ToArray();

            Assert.AreEqual(classes.Length, 1);
            Assert.AreEqual(classes[0], "ListGeneratorTest");
        }

        [TestMethod]
        public void TestMethodsCount()
        {
            var methods = CSharpSyntaxTree.ParseText(_result[0].TestClassCode).GetRoot().DescendantNodes()
                .OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(methods.Count(), 2);
        }

        [TestMethod]
        public void TestMethodsSignature()
        {
            var methods = CSharpSyntaxTree.ParseText(_result[0].TestClassCode).GetRoot().DescendantNodes()
                .OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                Assert.AreEqual(method.ReturnType.ToString(), "void");
                Assert.IsTrue(method.Modifiers.Any(SyntaxKind.PublicKeyword));
                Assert.AreEqual(method.AttributeLists[0].Attributes[0].Name.ToString(), "TestMethod");
            }
        }
    }
}