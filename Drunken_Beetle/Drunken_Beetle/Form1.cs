using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Drunken_Beetle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Room(TextBox[,] text, int size, int[,] save)  // 타일 생성
        {
            for (int i = 0; i < text.GetLength(0); i++)
            {
                for (int j = 0; j < text.GetLength(1); j++)
                {
                    text[i, j] = new TextBox();

                    text[i, j].Location = new Point(size + (j * 30), size + (i * 30));
                    text[i, j].Size = new Size(20, 10);
                    text[i, j].Name = i.ToString();
                    text[i, j].TabIndex = i;
                    text[i, j].TextAlign = HorizontalAlignment.Center;

                    save[i, j] = 0;
                    panel2.Controls.Add(text[i, j]);
                }
            }
        }
        private int Check(int move_count, int size)
        {
            if (move_count == (size * size))
                return 1;
            else
                return 0;
        }
        private int CheckRange(int nx, int ny, int size)
        {
            if (0 <= nx && nx < size && 0 <= ny && ny < size)
                return 1;
            else
                return 0;
        }
        private int BeetleMove(int x, int y, int nx, int ny, TextBox[,] text, int size, int count, int[,] save)
        {
            int move_count = 1;
            count = 1;
            int bx = x;
            int by = y;

            text[x, y].Text = "■";  // 출발 위치
            save[x, y] = 1;

            while (true)
            {
                Random rand = new Random();
                int pos = rand.Next(0, 8);

                int[] dx = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
                int[] dy = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };

                if (Check(move_count, size) == 1)
                {
                    save[x, y]++;
                    break;

                }
                nx = x + dx[pos];
                ny = y + dy[pos];

                if (CheckRange(nx, ny, size) == 1)
                {
                    bx = x;
                    by = y;

                    x = nx;
                    y = ny;

                    if (save[x, y] == 0)
                    {
                        move_count++;
                        ++save[x, y];
                    }

                    text[bx, by].Text = "■";  // 딱정벌레가 지나온 타일
                    text[x, y].Text = "□";  // 딱정벌레의 현재 위치

                    text[x, y].Refresh();
                    text[bx, by].Refresh();
                    Thread.Sleep(100);

                    count++;
                }
                text[x, y].Text = "■";
            }
            return count;
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            MessageBox.Show("// 술 취한 딱정벌레는 조건이 달라짐에 따라 딱정벌레가 방 안의 모든 타일을 한 번 이상 지나가는 데 걸리는 시간 및 통계 자료를 구하는 문제 입니다. \n" +
                "// 위의 조건은 사용자가 선택한 방의 크기와 딱정벌레의 출발 위치 \n" + " - 사용 절차 - \n" +
                "1. 게임 시작 메뉴를 선택한다. \n" + "2. 주어진 범위에 맞는 방의 크기와 딱정벌레의 출발 위치를 입력한다. \n" +
                "3. 위에서 입력한 조건으로 프로그램을 몇 번 실행할지 횟수를 입력한다. \n" + "4. 딱정벌레가 모든 방을 돈다. \n" +
                "5. 프로그램이 다 실행되면 결과값을 출력한다. \n" + "6. 다시 처음 메뉴가 나오면서 게임 설명, 시작, 종료 중 하나를 선택한다. \n" +
                "// 주의 사항 : 각 메뉴 당 선택 사항을 하나씩 고르시오 \n", "프로그램 설명");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string input1;
            string input2;
            string input3;
            int size = 0;
            int x = 0;
            int y = 0;
            int nx = 0;  // 이동할 위치
            int ny = 0;  // 이동할 위치
            int count = 0;
            int total_count = 0;

            do
            {
                input1 = Microsoft.VisualBasic.Interaction.InputBox("방의 크기 : ", "프로그램 시작", "ex. 5");
                size = Int32.Parse(input1);

                if (5 > size || size > 10)
                {
                    MessageBox.Show("방의 최소 5*5, 최대 10*10 입니다.");
                }
            } while (5 > size || size > 10);

            do
            {
                input2 = Microsoft.VisualBasic.Interaction.InputBox("출발 위치(X) : ", "프로그램 시작", "ex. 1");
                x = Int32.Parse(input2);  // 현재 위치

                if (x == 0 || x > size)
                {
                    MessageBox.Show("범위를 다시 확인해주세요.");
                }
            } while (x == 0 || x > size);

            do
            {
                input3 = Microsoft.VisualBasic.Interaction.InputBox("출발 위치(Y) : ", "프로그램 시작", "ex. 1");
                y = Int32.Parse(input3);  // 현재 위치

                if (y == 0 || y > size)
                {
                    MessageBox.Show("범위를 다시 확인해주세요.");
                }
            } while (y == 0 || y > size);

            TextBox[,] text = new TextBox[size, size];
            int[,] save = new int[size, size];
            Stopwatch clock = new Stopwatch();

            Room(text, size, save);

            x -= 1;
            y -= 1;

            clock.Start();
            total_count = BeetleMove(x, y, nx, ny, text, size, count, save);
            clock.Stop();

            double total_time = clock.ElapsedMilliseconds;

            textBox2.Text = input1;
            textBox3.Text = input2;
            textBox4.Text = input3;
            textBox5.Text = Convert.ToString(total_count);
            textBox6.Text = Convert.ToString(total_time + " ms");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("프로그램을 종료합니다.", "프로그램 종료");
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
