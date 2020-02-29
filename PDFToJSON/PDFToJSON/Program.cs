using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFToJSON
{
    class Program
    {
        class Question
        {
            public string QuestionText { get; set; }
            public string Answer1 { get; set; }
            public string Answer2 { get; set; }
            public string Answer3 { get; set; }
            public string Answer4 { get; set; }
            public int CorrectIdx { get; set; }
        }

        static void Main(string[] args)
        {
            //Prepare the input text file by:
            //1) Copy paste the text from the original PDF file
            //2) Remove the lines with the page numbers
            //3) Some questions contain images in their answers and they are blank. Just make sure they are on separate lines and have dots - A. B.
            Console.Write("Input filename: ");
            var inputFilename = Console.ReadLine();

            var lines = ReadLines(inputFilename);
            var questions = ParseInputLines(lines);
            var output = RenderQuestions(questions);

            System.IO.File.WriteAllText(@"output.txt", output);
        }

        private static string[] ReadLines(string inputFilePath)
        {
            return System.IO.File.ReadAllLines(inputFilePath);
        }

        private static List<Question> ParseInputLines(string[] inputLines)
        {
            var questions = new List<Question>();

            //The question is on the first line, the next four lines contain the answers of each questions
            for (int i = 0; i <= inputLines.Length - 5; i += 5)
            {
                var question = new Question();

                var questionLine = inputLines[i].Trim();

                //The correct answer is at the end of the question line
                var correctAnswer = questionLine.Substring(questionLine.Length - 3);
                var correctAnswerIdx = 0;
                //Most of the correct answers are in cyrilic. There are a few one in english letters - I corrected those in the final JSON
                if (correctAnswer == "(А)") correctAnswerIdx = 1;
                else if (correctAnswer == "(Б)") correctAnswerIdx = 2;
                else if (correctAnswer == "(В)") correctAnswerIdx = 3;
                else if (correctAnswer == "(Г)") correctAnswerIdx = 4;

                //Remove the correct answer part at the end (e.g. " (A)")
                questionLine = questionLine.Substring(0, questionLine.Length - 3).Trim();

                //Remove the index of the question at the start (e.g. "1. ")
                questionLine = questionLine.Substring(questionLine.IndexOf(" ")).Trim();

                question.QuestionText = questionLine;
                //Remove the letter index (e.g. "A. " from each answer
                question.Answer1 = inputLines[i + 1].Trim().Substring(2).Trim();
                question.Answer2 = inputLines[i + 2].Trim().Substring(2).Trim();
                question.Answer3 = inputLines[i + 3].Trim().Substring(2).Trim();
                question.Answer4 = inputLines[i + 4].Trim().Substring(2).Trim();
                question.CorrectIdx = correctAnswerIdx;

                questions.Add(question);
            }

            return questions;
        }

        private static string RenderQuestions(List<Question> questions)
        {
            var output = "";
            var questionIdx = 0;
            foreach (var q in questions)
            {
                questionIdx++;
                output += @"
{
	""id"": " + questionIdx.ToString() + @",
	""question"": """ + q.QuestionText + @""",
	""answers"": [
		{
			""answer"": """ + q.Answer1 + @"""" + (q.CorrectIdx == 1 ? @",
			""isCorrect"": true" : "") + @"
		},
		{
			""answer"": """ + q.Answer2 + @"""" + (q.CorrectIdx == 2 ? @",
			""isCorrect"": true" : "") + @"
		},
		{
			""answer"": """ + q.Answer3 + @"""" + (q.CorrectIdx == 3 ? @",
			""isCorrect"": true" : "") + @"
		},
		{
			""answer"": """ + q.Answer4 + @"""" + (q.CorrectIdx == 4 ? @",
			""isCorrect"": true" : "") + @"
		}
	]
},";
            }

            return output;
        }
    }
}
