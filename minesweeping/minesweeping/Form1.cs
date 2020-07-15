using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Media;

namespace saolei
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //定义问号图片路径
        private string wenhao = System.Environment.CurrentDirectory + "\\image\\ask.bmp";
        //定义初始后开始按钮的图片
        private string chushi = System.Environment.CurrentDirectory + "\\image\\face.bmp";
        //定义炸死后开始按钮的图片
        private string siqu = System.Environment.CurrentDirectory + "\\image\\sile.jpg";
        //定义胜利的声音
        private string shengli = System.Environment.CurrentDirectory + "\\image\\烟花.wav";
        //定义爆炸声音
        private string shengyin=System.Environment.CurrentDirectory+"\\image\\爆炸声.wav";
        //定义失败时，雷炸开图片的路径
        private string shibai = System.Environment.CurrentDirectory + "\\image\\shibai.bmp";
        //定义右键单击时，标记旗子图片的路径
        private string flag = System.Environment.CurrentDirectory + "\\image\\flag.bmp";
        //地雷图片位置
        private string dileiimage = System.Environment.CurrentDirectory + "\\imag\\lei.bmp";
        //计时器
        private Timer timer1 = new Timer();
        //所用时间
        private int yongshi = 0;
        //定义地雷数
        private  int leishu = 50;
        //游戏是否结束
        private  bool over = false;
        //生成雷的行数
        private int hang = 16;
        //生成雷的列数
        private  int lie = 30;
        //游戏过程中剩余的地雷数量
        private int restlie; 

        //定义属性
        public int Leishu
        {
            get
            { return leishu; }
            set
            {
                leishu= value;
            }
        
        }
        public int Hang
        {
            get
            { return hang; }
            set
            {
                hang = value;
            }
        }
        public int Lie
        {
            get
            { return lie; }
            set
            {
                lie = value;
            }
        }


        //生成个按钮数组        
        private LeiButton[,] button = new LeiButton[16, 30];


        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 设置地雷数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shezhi shezhi1 = new shezhi();
            shezhi1.ShowDialog();
            

        }
        /// <summary>
        /// 窗体的load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
           
            
            Program.form = this; restlie = Leishu;
            label4.Text = "";
            groupBox1.Location = new Point(26, 26);
            groupBox1.Text = "";
            groupBox1.Size = new System.Drawing.Size(908, 488);
            groupBox1.FlatStyle = FlatStyle.Standard;
            lei.Text = "  "+restlie.ToString() + "颗";
            this.Location = new Point(20, 20);
            timer1.Enabled = true;
            Leizheng();
            Bulei();
            this.StartPosition = FormStartPosition.Manual;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
        }
        /// <summary>
        /// 定义timer组件的Tick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            yongshi++;
            if (yongshi < 60)
                  label4.Text = yongshi.ToString() + "秒";
            else
                label4.Text = (yongshi /60).ToString() + "分" + (yongshi % 60).ToString() + "秒";

        }


        /// <summary>
        /// 生成雷阵的界面，即生成LeiButton控件（继承于Button控件）
        /// </summary>
        private void Leizheng()
        {
            for (int i = 0; i < lie; i++)
            {
                for (int j = 0; j < hang; j++)
                {
                    button[j, i] = new LeiButton();
                    button[j, i].Location = new Point( 3+i * 30, 6+ j * 30);
                    button[j, i].X = j;
                    button[j, i].Y = i;
                    button[j, i].Youlei = 0;
                    button[j, i].Font = new System.Drawing.Font("宋体",button[j,i].Font.Size,button[j,i].Font.Style);
                    
                   
                    groupBox1.Controls.Add(button[j, i]);
                    button[j, i].MouseUp += new MouseEventHandler(bt_MouseUp);
                    
                    //   .Location = new Point(30+i * 30, 30+j * 30);
                    // bt.X = j; bt.Y = i; bt.Youlei = 0;
                    // button[j, i] = bt;
                    // //bt.Text = i.ToString() + "" + j.ToString();
                    //bt.MouseUp+=new MouseEventHandler(bt_MouseUp);
                    // this.Controls.Add(bt);

                }
            }
        }
        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
                timer1.Enabled = true;
                
                Fuyuan();
               
            
        }
        /// <summary>
        /// 定义鼠标单击事件,单击按钮时触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_MouseUp(object sender, MouseEventArgs e)
        {
            if (!over)
            {
                int x, y;
                //获取被点击的Button按钮
                LeiButton b = (LeiButton)sender;
                x = b.X;//x代表button数组的第一个索引
                y = b.Y;//y表示Button数组的第二个索引
                //判断按下的鼠标键是哪个
                switch (e.Button)
                {
                    //按下鼠标左键
                    case MouseButtons.Left:
                        //判断该方格是否被翻开，Tag=0表示方格未被翻开
                        if (Convert.ToInt16(button[x, y].Tag) == 0)
                        {
                            if (button[x, y].Youlei == 0)
                            {
                                button[x, y].Enabled = false;
                                button[x, y].Text = Getdilei(x, y).ToString();
                                Saolei(x, y);
                                if (Win())
                                {

                                    Showlei();
                                    timer1.Enabled = false;
                                    SoundPlayer sound = new SoundPlayer(shengli);
                                    sound.Play();
                                    MessageBox.Show("恭喜你扫雷成功！游戏结束！", "扫雷完成");
                                    over = true;
                                }

                            }
                            else
                            {
                                button[x, y].BackgroundImage = Image.FromFile(shibai);
                                SoundPlayer sound = new SoundPlayer(shengyin);
                                sound.Play();
                                timer1.Enabled = false;
                                b.Enabled = false;
                                b.BackgroundImage = Image.FromFile(shibai);
                                xianshi(); 
                                //button1.Image = Image.FromFile(siqu);
                                //button1.ImageAlign = ContentAlignment.MiddleRight;
                                MessageBox.Show("回家练练运气再来！", "游戏失败");
                              
                                over = true;

                            }
                        }
                        break;
                    case MouseButtons.Right:
                       
                        if (Convert.ToInt16(button[x, y].Tag) == 1)
                        {
                            button[x, y].Tag = 2;
                            button[x, y].BackgroundImage = Image.FromFile(wenhao);

                        }
                        else if (Convert.ToInt16(button[x, y].Tag) == 2)
                        {
                            button[x, y].Tag = 0;
                            restlie++;
                            button[x, y].BackgroundImage = null;
                        }
                        else
                        {
                            button[x, y].Tag = 1;
                            button[x, y].BackgroundImage = Image.FromFile(flag);
                            restlie--;
                        
                        }
                        lei.Text ="  "+ restlie.ToString() + "颗";
                        if (Win())
                        {
                            SoundPlayer sound = new SoundPlayer(shengli);
                            sound.Play();
                            MessageBox.Show("恭喜你！你太有才了，扫雷成功","扫雷完成");
                            timer1.Enabled = false;
                            over = true;
                        }
                        break;

                }
            }
            else
                return;


        }
        /// <summary>
        ///  //动态布置地雷,产生随机数布雷
        /// </summary>
        private void Bulei()
        {
            Random rand = new Random();
            for (int i = 0; i < leishu; i++)
            {

                int position_x = rand.Next(hang);
                int position_y = rand.Next(lie);
                if (button[position_x, position_y].Youlei == 0)
                {
                    button[position_x, position_y].Youlei = 1;

                }
                else
                    i = i - 1;
            }

        }
        /// <summary>
        /// /判断点开的这个按钮周围8个中有几个地雷
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Getdilei(int row, int col)//x代表行，y代表列
        {
            int i, j;
            int around = 0;
            int minRow = (row == 0) ? 0 : row - 1;
            int maxRow = row + 2;
            int minCol = (col == 0) ? 0 : col - 1;
            int maxCol = col + 2;
            for (i = minRow; i < maxRow; i++)
            {
                for (j = minCol; j < maxCol; j++)
                {
                    if (!(i>= 0 && i < hang && j >= 0 && j < lie))//判断是否在扫雷区域
                        continue;
                    if (button[i, j].Youlei == 1) around++;
                }
            }
            return around;
        }
      
        /// <summary>
        ///以下递归扫雷,向周围八个发散的递归查找
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void Saolei(int row, int col)

        {

            int minrow1 = (row == 0) ? 0 : row - 1;
            int mincol1 = (col == 0) ? 0 : col - 1;
            int maxrow1 = row + 2;
            int maxcol1 = col + 2;
            int leishuliang = Getdilei(row, col);
            if (leishuliang == 0)
            {
                button[row, col].Enabled = false;
                for (int m = minrow1; m < maxrow1; m++)
                {
                    for (int n = mincol1; n < maxcol1; n++)
                    {
                        if (!(m >= 0 && m < hang && n >= 0 && n < lie))
                            continue;
                        if (!(m == row && n == col) && button[m, n].Enabled == true&&Convert.ToInt16(button[m, n].Tag) == 0)
                            Saolei(m, n);
                        //判断该处是否标记为有雷，有雷该处不作任何变化，无雷控件Enable属性变为false
                        if (Convert.ToInt16(button[m, n].Tag) == 0)
                          button[m, n].Enabled = false;
                        button[m, n].Text = Getdilei(m, n).ToString();
                        if (button[m, n].Text == "0")
                            button[m, n].Text = string.Empty;
                    }

                }
            }

        }
        /// <summary>
        /// 判断是否扫完地雷
        /// </summary>
        /// <returns></returns>
        private bool Win()
        {   int zongshu=0;
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                {  
                    if (button[i, j].Youlei == 1 && Convert.ToInt16(button[i, j].Tag) == 1)
                        zongshu++;

                }

            }
            if (zongshu == leishu&&restlie==0)
                return true;
            else
                return false;             

        }

        private void Showlei()
        {
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                    if (button[i, j].Youlei == 1)
                    {
                        
                            button[i, j].BackgroundImage = Image.FromFile(dileiimage);
                     
                    }

            }

        }

        private void xianshi()
        {
            int l = 0;
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                {
                    if (button[i, j].Youlei == 1)
                    {
                        button[i, j].BackgroundImage = Image.FromFile(shibai);
                        l++; 
                        //MessageBox.Show(button[i,j].Youlei.ToString());
                    }
                }
            }
           
        }

        /// <summary>
        /// 每次按开始按钮时，所有方块复原
        /// </summary>
        public void Fuyuan()
        {
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                {
                    button[i, j].Tag = 0;
                    button[i, j].Enabled = true;
                    button[i, j].Text = string.Empty;
                    button[i, j].BackgroundImage = null;
                    if (button[i, j].Youlei == 1)
                        button[i, j].Youlei = 0;
                }
            
            }
            Bulei();
            yongshi = 0;
            over = false;
            restlie = leishu;
            lei.Text = "  "+restlie.ToString() + "颗";
            
            
           
        }

        private void 关于此游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此游戏由XXX制作！","关于此游戏");
        }

        private void 游戏规则ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string messagestring = "扫雷是一种具有迷惑性的对记忆和推理能力的简单测试，它是长久以来最受欢迎的 Windows 游戏之一。游戏目标：找出空方块并避免触雷。听起来很容易，那就试试吧！";
            messagestring += "\n玩法\n";
            messagestring += "1、挖开地雷，游戏即告结束。\n";
            messagestring += "2、挖开空方块，可以继续玩。\n";
            messagestring += "3、挖开数字，则表示在其周围的八个方块中共有多少个雷，可以使用该信息推断能够安全单击附近的哪些方块。";
            MessageBox.Show(messagestring,"查看帮助");


        }

        private void 联机帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能陆续开发中，请耐心等待！","联机帮助");
        }


      
    }
}
