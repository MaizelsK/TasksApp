using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TasksApp
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Начало работы
        private void StartTaskBtnClick(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            // Отключение кнопки 'Начать работу'
            StartTaskBtn.IsEnabled = false;

            Task newTask = new Task(new Action(DoWork), cancellationToken);
            newTask.Start();
        }

        // Имитация работы
        public void DoWork()
        {
            Random random = new Random();

            int workTime = random.Next(10000, 20000);

            // Выставление максимального значения Progress Bar
            Dispatcher.Invoke(() =>
            {
                TaskProgress.Maximum = workTime;
            });

            // Прогресс работы через каждые 0.2 секунды
            for (int i = 0; i < workTime; i += 200)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                Dispatcher.Invoke(() =>
                {
                    TaskProgress.Value += 200;
                });

                Thread.Sleep(200);
            }

            Dispatcher.Invoke(() =>
            {
                ClearProgressBar();
            });
        }

        // Отмена работы
        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();

            ClearProgressBar();
        }

        // Обнуление Progress Bar
        private void ClearProgressBar()
        {
            StartTaskBtn.IsEnabled = true;
            TaskProgress.Value = 0;
        }
    }
}
