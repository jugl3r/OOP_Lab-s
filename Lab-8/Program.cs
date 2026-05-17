using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab_8
{
    // Статический класс для хранения введенных данных (состояние приложения)
    static class AppState
    {
        public static double SideA = 0;
        public static double SideB = 0;
        public static double SideC = 0;
        public static bool CalcPerimeter = false;
        public static bool CalcArea = false;
    }

    // Окно для ввода данных
    public class InputDialog : Form
    {
        private TextBox txtA;
        private TextBox txtB;
        private TextBox txtC;
        private CheckBox chkPerimeter;
        private CheckBox chkArea;
        private Button btnOk;

        public InputDialog()
        {
            this.Text = "Ввод данных треугольника";
            this.Size = new Size(300, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblA = new Label() { Text = "Сторона A:", Location = new Point(10, 15), AutoSize = true };
            txtA = new TextBox() { Location = new Point(100, 12), Width = 100, Text = AppState.SideA > 0 ? AppState.SideA.ToString() : "" };

            Label lblB = new Label() { Text = "Сторона B:", Location = new Point(10, 45), AutoSize = true };
            txtB = new TextBox() { Location = new Point(100, 42), Width = 100, Text = AppState.SideB > 0 ? AppState.SideB.ToString() : "" };

            Label lblC = new Label() { Text = "Сторона C:", Location = new Point(10, 75), AutoSize = true };
            txtC = new TextBox() { Location = new Point(100, 72), Width = 100, Text = AppState.SideC > 0 ? AppState.SideC.ToString() : "" };

            chkPerimeter = new CheckBox() { Text = "Периметр", Location = new Point(10, 110), Checked = AppState.CalcPerimeter };
            chkArea = new CheckBox() { Text = "Площадь", Location = new Point(100, 110), Checked = AppState.CalcArea };

            btnOk = new Button() { Text = "Сохранить", Location = new Point(100, 160) };
            btnOk.Click += BtnOk_Click;

            this.Controls.AddRange(new Control[] { lblA, txtA, lblB, txtB, lblC, txtC, chkPerimeter, chkArea, btnOk });
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                double a = Convert.ToDouble(txtA.Text);
                double b = Convert.ToDouble(txtB.Text);
                double c = Convert.ToDouble(txtC.Text);

                if (a <= 0 || b <= 0 || c <= 0) 
                    throw new Exception("Стороны должны быть больше нуля!");
                if (a + b <= c || a + c <= b || b + c <= a) 
                    throw new Exception("Треугольник с такими сторонами не существует!");

                // Сохраняем значения
                AppState.SideA = a;
                AppState.SideB = b;
                AppState.SideC = c;
                AppState.CalcPerimeter = chkPerimeter.Checked;
                AppState.CalcArea = chkArea.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Окно для вывода результатов
    public class CalcDialog : Form
    {
        public CalcDialog()
        {
            this.Text = "Результаты вычислений";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblResult = new Label() { Location = new Point(10, 20), AutoSize = true };
            
            string resultText = $"Треугольник со сторонами {AppState.SideA}, {AppState.SideB}, {AppState.SideC}\n\n";
            double p = AppState.SideA + AppState.SideB + AppState.SideC;

            if (AppState.CalcPerimeter)
            {
                resultText += $"Периметр: {p}\n";
            }

            if (AppState.CalcArea)
            {
                double hp = p / 2.0; // полупериметр
                double area = Math.Sqrt(hp * (hp - AppState.SideA) * (hp - AppState.SideB) * (hp - AppState.SideC));
                resultText += $"Площадь: {area:F2}\n";
            }

            if (!AppState.CalcPerimeter && !AppState.CalcArea)
            {
                resultText += "Вы не выбрали ни периметр, ни площадь для расчета.";
            }

            lblResult.Text = resultText;

            Button btnOk = new Button() { Text = "ОК", Location = new Point(100, 120) };
            btnOk.Click += (s, e) => this.Close();

            this.Controls.Add(lblResult);
            this.Controls.Add(btnOk);
        }
    }

    // Главное окно с меню
    public class MainForm : Form
    {
        public MainForm()
        {
            this.Text = "Лабораторная работа 8 (Вариант 1)";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();
            
            ToolStripMenuItem menuInput = new ToolStripMenuItem("Input");
            menuInput.Click += (s, e) => {
                InputDialog inputDialog = new InputDialog();
                inputDialog.ShowDialog(this);
            };

            ToolStripMenuItem menuCalc = new ToolStripMenuItem("Calc");
            menuCalc.Click += (s, e) => {
                if (AppState.SideA == 0) 
                {
                    MessageBox.Show("Сначала введите длины сторон (меню Input)!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CalcDialog calcDialog = new CalcDialog();
                calcDialog.ShowDialog(this);
            };

            ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
            menuExit.Click += (s, e) => Application.Exit();

            menuStrip.Items.Add(menuInput);
            menuStrip.Items.Add(menuCalc);
            menuStrip.Items.Add(menuExit);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
            
            Label infoLabel = new Label() {
                Text = "Выберите пункт меню Input для ввода данных\nили Calc для вычислений.",
                Location = new Point(50, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            this.Controls.Add(infoLabel);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
