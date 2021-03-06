﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalseQuiz.Models;
using TrueOrFalseQuiz.Views;
using Xamarin.Forms;

namespace TrueOrFalseQuiz.ViewModels
{
    class QuizPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public List<Question> questions;

        private string _currentQuestionText;
        public string CurrentQuestionText {
            get
            {
                return _currentQuestionText;
            }

            set
            {
                _currentQuestionText = value;
                OnPropertyChanged();
            }
        }

        private bool _currentAnswerValue;
        public bool CurrentAnswerValue
        {
            get
            {
                return _currentAnswerValue;
            }

            set
            {
                _currentAnswerValue = value;
                OnPropertyChanged();
            }
        }

        private int _totalQuestions;
        public int TotalQuestions
        {
            get
            {
                return _totalQuestions;
            }

            set
            {
                _totalQuestions = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TitleText));
            }
        }

        private int _currentQuestionNumber;
        public int CurrentQuestionNumber
        {
            get
            {
                return _currentQuestionNumber;
            }

            set
            {
                _currentQuestionNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TitleText));
            }
        }

        public string TitleText
        {
            get { return $"Question {_currentQuestionNumber} of {_totalQuestions}"; }
        }
        
        private int score;

        private Random random;

        public Command AnsweredTrue { get; }
        public Command AnsweredFalse { get; }
        
        public QuizPageModel()
        {
            // initialise RNG
            random = new Random();

            // populate question list - replace with external data source in production
            questions = new List<Question>()
            {
                new Question() { QuestionText="This fact is true", Answer=true },
                new Question() { QuestionText="This fact is false", Answer=false },
                new Question() { QuestionText="QuestionText is a string", Answer=true },
                new Question() { QuestionText="AnswerValue is also a string", Answer=false },
                new Question() { QuestionText="These questions are stored in a list", Answer=true },
                new Question() { QuestionText="I need at least ten questions", Answer=true },
                new Question() { QuestionText="This app is made with Java", Answer=false },
                new Question() { QuestionText="This is the ninth fact in the list", Answer=false },
                new Question() { QuestionText="I am running out of ideas for questions", Answer=true },
                new Question() { QuestionText="This is the last fact", Answer=true }
            };

            // initialise quiz values
            TotalQuestions = questions.Count;
            CurrentQuestionNumber = 1;
            score = 0;

            // load first question
            LoadQuestion();

            AnsweredTrue = new Command(async () =>
            {
                Debug.WriteLine("True button pressed");
                
                // check if answer is correct
                if (_currentAnswerValue == true) score++;

                // load next question or results page
                if (CurrentQuestionNumber < TotalQuestions)
                {
                    // increase question counter
                    CurrentQuestionNumber++;
                    LoadQuestion();
                }
                else
                {
                    Debug.WriteLine("End of Quiz");
                    await ShowResults().ConfigureAwait(false);
                }
            });

            AnsweredFalse = new Command(async () =>
            {
                Debug.WriteLine("False button pressed");
                
                // check if answer is correct
                if (_currentAnswerValue == false) score++;
                
                // load next question or results page
                if (CurrentQuestionNumber < TotalQuestions)
                {
                    // increase question counter
                    CurrentQuestionNumber++;
                    LoadQuestion();
                } 
                else
                {
                    Debug.WriteLine("End of Quiz");
                    await ShowResults().ConfigureAwait(false);
                }
            });
        }

        private void LoadQuestion()
        {
            var index = random.Next(questions.Count);
            CurrentQuestionText = questions[index].QuestionText;
            CurrentAnswerValue = questions[index].Answer;
            questions.RemoveAt(index);
        }

        private async Task ShowResults() => await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(score, _totalQuestions)).ConfigureAwait(false);
    }
}
