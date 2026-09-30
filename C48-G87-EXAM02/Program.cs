using System;

class program
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public Answer(int answerId, string answerText)
        {
            AnswerId = answerId;
            AnswerText = answerText;
        }

        public override string ToString()
        {
            return $"{AnswerId}. {AnswerText}";
        }
    }

    public class Question
    {
        protected string header { get; set; }
        public string body { get; set; }

        protected double mark;

        public double Mark
        {
            get { return mark; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Mark cannot be negative.");
                }

                mark = value;
            }
        }

        public Answer[] answerList;
        public Answer CorrectAnswer { get; set; }

        public Question(
            string Header,
            string Body,
            double Mark,
            Answer[] AnswerList,
            Answer CorrectAnswer)
        {
            header = Header;
            body = Body;
            this.Mark = Mark;
            answerList = AnswerList;
            this.CorrectAnswer = CorrectAnswer;
        }

        public override string ToString()
        {
            return body;
        }
    }

    public class MCQ : Question
    {
        public MCQ(
            string Header,
            string Body,
            double Mark,
            Answer CorrectAnswer,
            Answer[] AnswerList)
            : base(Header, Body, Mark, AnswerList, CorrectAnswer)
        {
        }
    }

    public class TrueFalse : Question
    {
        public TrueFalse(
            string Header,
            string Body,
            double Mark,
            Answer CorrectAnswer)
            : base(
                Header,
                Body,
                Mark,
                new Answer[]
                {
                    new Answer(1, "True"),
                    new Answer(2, "False")
                },
                CorrectAnswer)
        {
        }
    }

    public abstract class Exam
    {
        public DateTime TimeOfExam;
        public int NumOfQuestions;
        public Question[] questions;

        public Exam(DateTime timeOfExam, int numOfQuestions)
        {
            TimeOfExam = timeOfExam;
            NumOfQuestions = numOfQuestions;
            questions = new Question[NumOfQuestions];
        }

        public void AddQuestions(Question[] questions)
        {
            for (int i = 0; i < NumOfQuestions; i++)
            {
                this.questions[i] = questions[i];
            }
        }

        public abstract void showExam();
    }

    public class PracticalExam : Exam
    {
        public PracticalExam(DateTime timeOfExam, int numOfQuestions)
            : base(timeOfExam, numOfQuestions)
        {
        }

        public override void showExam()
        {
            for (int i = 0; i < NumOfQuestions; i++)
            {
                Console.WriteLine(
                    "\nQuestion " + (i + 1) + ": " + questions[i].body
                );

                for (int j = 0; j < questions[i].answerList.Length; j++)
                {
                    Console.WriteLine(
                        "Answer: " + questions[i].answerList[j].AnswerText
                    );
                }
            }

            Console.WriteLine("\nCorrect Answers:");

            for (int i = 0; i < NumOfQuestions; i++)
            {
                Console.WriteLine(
                    "The correct answer for Question " + (i + 1) +
                    " is " + questions[i].CorrectAnswer.AnswerText
                );
            }
        }
    }

    public class FinalExam : Exam
    {
        public FinalExam(DateTime timeOfExam, int numOfQuestions)
            : base(timeOfExam, numOfQuestions)
        {
        }

        public override void showExam()
        {
            double totalGrade = 0;

            for (int i = 0; i < NumOfQuestions; i++)
            {
                Console.WriteLine(
                    "\nQuestion: " + questions[i].body +
                    "\nCorrect Answer: " + questions[i].CorrectAnswer.AnswerText +
                    "\nGrade: " + questions[i].Mark
                );

                totalGrade += questions[i].Mark;
            }

            Console.WriteLine("\nTotal Grade: " + totalGrade);
        }
    }

    public class Subject
    {
        int SubjectId;
        string SubjectName;

        public Subject(int subjectId, string subjectName)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
        }

        public Exam createExam()
        {
            Console.WriteLine(
                "Please enter type of exam: enter 0 for practical and 1 for final exam:"
            );

            int examType = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter the name of the subject:");
            SubjectName = Console.ReadLine();

            Console.WriteLine("Please enter the ID of the subject:");
            SubjectId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter date and time of the exam dd/MM/yyyy HH:mm:");
            DateTime timeOfExam = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the number of questions in the exam:");
            int numOfQuestions = Convert.ToInt32(Console.ReadLine());

            // Create ONE exam before creating the questions.
            Exam exam;

            if (examType == 0)
            {
                exam = new PracticalExam(timeOfExam, numOfQuestions);
            }
            else
            {
                exam = new FinalExam(timeOfExam, numOfQuestions);
            }

            // Create all questions for that ONE exam.
            for (int i = 0; i < numOfQuestions; i++)
            {
                Console.WriteLine(
                    $"\nPlease enter the body of question {i + 1}:"
                );

                string questionBody = Console.ReadLine();

                Console.WriteLine(
                    $"Please enter the mark for question {i + 1}:"
                );

                double questionMark = Convert.ToDouble(Console.ReadLine());

                Answer[] answerList;

                if (examType == 0)
                {
                    // Practical exam questions are MCQs.
                    Console.WriteLine(
                        $"Please enter the number of answers for question {i + 1}:"
                    );

                    int numOfAnswers = Convert.ToInt32(Console.ReadLine());

                    answerList = new Answer[numOfAnswers];

                    for (int j = 0; j < numOfAnswers; j++)
                    {
                        Console.WriteLine(
                            $"Please enter the text for answer {j + 1}:"
                        );

                        string answerText = Console.ReadLine();

                        answerList[j] = new Answer(j + 1, answerText);
                    }

                    Console.WriteLine(
                        $"Please enter the number of the correct answer (1-{numOfAnswers}):"
                    );

                    int correctAnswerIndex =
                        Convert.ToInt32(Console.ReadLine()) - 1;

                    Answer correctAnswer = answerList[correctAnswerIndex];

                    exam.questions[i] = new MCQ(
                        $"Question {i + 1}",
                        questionBody,
                        questionMark,
                        correctAnswer,
                        answerList
                    );
                }
                else
                {
                    // For now, final-exam questions are True/False.
                    answerList = new Answer[]
                    {
                        new Answer(1, "True"),
                        new Answer(2, "False")
                    };

                    Console.WriteLine(
                        "Please enter the correct answer: 1 for True, 2 for False"
                    );

                    int correctAnswerId = Convert.ToInt32(Console.ReadLine());

                    Answer correctAnswer = answerList[correctAnswerId - 1];

                    exam.questions[i] = new TrueFalse(
                        $"Question {i + 1}",
                        questionBody,
                        questionMark,
                        correctAnswer
                    );
                }
            }

            return exam;
        }
    }

    static void Main(string[] args)
    {
        Subject subject = new Subject(1, "English");

        Exam exam = subject.createExam();

        if (exam is PracticalExam practical)
        {
            Console.WriteLine("\nPractical Exam created successfully.");
            practical.showExam();
        }
        else if (exam is FinalExam final)
        {
            Console.WriteLine("\nFinal Exam created successfully.");
            final.showExam();
        }
    }
}