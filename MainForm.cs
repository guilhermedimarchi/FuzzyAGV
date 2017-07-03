// Fuzzy Auto Guided Vehicle Sample
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Fabio L. Caversan, 2008-2009
// fabio.caversan@gmail.com
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace FuzzyAGV
{
    public class MainForm : System.Windows.Forms.Form
    {

        #region Private members
        private string RunLabel;
        private Point InitialPos;
        private bool FirstInference;
        private int LastX;
        private int LastY;
        private int Time = 0;
        private double Angle, Speed;
        private Bitmap OriginalMap, InitialMap;
        private Thread thMovement;
        private FIS fis;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.PictureBox pbTerrain;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.CheckBox cbLasers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label txtRight;
        private System.Windows.Forms.Label txtLeft;
        private System.Windows.Forms.Label txtFront;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label txtAngle;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbRobot;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox gbComandos;
        private IContainer components;
        private GroupBox groupBox3;
        private Label label5;
        private Button aboutButton;
        private Label txtSpeed;
        private Label label6;
        private System.Windows.Forms.Timer timer;
        private Label txtTime;
        private Label label8;
        private System.Windows.Forms.CheckBox cbTrajeto;
        #endregion

        #region Class constructor, destructor and Main method

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main( )
        {
            Application.Run( new MainForm( ) );
        }


        public MainForm( )
        {
            InitializeComponent( );
            Angle = 0;
            OriginalMap = new Bitmap( pbTerrain.Image );
            InitialMap = new Bitmap( pbTerrain.Image );

            fis = new FIS();
            FirstInference = true;
            pbRobot.Top = pbTerrain.Bottom - 50;
            pbRobot.Left = pbTerrain.Left + 60;
            InitialPos = pbRobot.Location;
            RunLabel = btnRun.Text;
        }

        /// <summary>
        /// Stoping the movement thread
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            StopMovement( );
            if ( disposing )
            {
                if ( components != null )
                {
                    components.Dispose( );
                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pbTerrain = new System.Windows.Forms.PictureBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.cbLasers = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRight = new System.Windows.Forms.Label();
            this.txtLeft = new System.Windows.Forms.Label();
            this.txtFront = new System.Windows.Forms.Label();
            this.lbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.Label();
            this.gbComandos = new System.Windows.Forms.GroupBox();
            this.cbTrajeto = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pbRobot = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.aboutButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.txtTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbComandos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbTerrain
            // 
            this.pbTerrain.BackColor = System.Drawing.SystemColors.ControlText;
            this.pbTerrain.ErrorImage = null;
            this.pbTerrain.Image = ((System.Drawing.Image)(resources.GetObject("pbTerrain.Image")));
            this.pbTerrain.InitialImage = null;
            this.pbTerrain.Location = new System.Drawing.Point(160, 8);
            this.pbTerrain.Name = "pbTerrain";
            this.pbTerrain.Size = new System.Drawing.Size(500, 500);
            this.pbTerrain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTerrain.TabIndex = 10;
            this.pbTerrain.TabStop = false;
            this.pbTerrain.Click += new System.EventHandler(this.pbTerrain_Click);
            this.pbTerrain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseDown);
            this.pbTerrain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseMove);
            // 
            // btnStep
            // 
            this.btnStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep.Location = new System.Drawing.Point(6, 109);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 14;
            this.btnStep.Text = "&One Step";
            this.btnStep.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnRun
            // 
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Location = new System.Drawing.Point(6, 138);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 15;
            this.btnRun.Text = "&Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(6, 83);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(72, 20);
            this.txtInterval.TabIndex = 16;
            this.txtInterval.Text = "10";
            this.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbLasers
            // 
            this.cbLasers.Checked = true;
            this.cbLasers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLasers.Location = new System.Drawing.Point(8, 40);
            this.cbLasers.Name = "cbLasers";
            this.cbLasers.Size = new System.Drawing.Size(120, 24);
            this.cbLasers.TabIndex = 17;
            this.cbLasers.Text = "&Show Beams";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRight);
            this.groupBox1.Controls.Add(this.txtLeft);
            this.groupBox1.Controls.Add(this.txtFront);
            this.groupBox1.Controls.Add(this.lbl);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 72);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor readings::";
            // 
            // txtRight
            // 
            this.txtRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRight.Location = new System.Drawing.Point(104, 48);
            this.txtRight.Name = "txtRight";
            this.txtRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtRight.Size = new System.Drawing.Size(32, 16);
            this.txtRight.TabIndex = 29;
            this.txtRight.Text = "0";
            this.txtRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLeft
            // 
            this.txtLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLeft.Location = new System.Drawing.Point(104, 32);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtLeft.Size = new System.Drawing.Size(32, 16);
            this.txtLeft.TabIndex = 28;
            this.txtLeft.Text = "0";
            this.txtLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFront
            // 
            this.txtFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront.Location = new System.Drawing.Point(104, 16);
            this.txtFront.Name = "txtFront";
            this.txtFront.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront.Size = new System.Drawing.Size(32, 16);
            this.txtFront.TabIndex = 27;
            this.txtFront.Text = "0";
            this.txtFront.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl
            // 
            this.lbl.Location = new System.Drawing.Point(8, 48);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(100, 16);
            this.lbl.TabIndex = 26;
            this.lbl.Text = "Right (pixels):";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "Left (pixels):";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 24;
            this.label1.Text = "Frontal (pixels):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTime);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtSpeed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtAngle);
            this.groupBox2.Location = new System.Drawing.Point(8, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 72);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Angle (degrees):";
            // 
            // txtAngle
            // 
            this.txtAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAngle.Location = new System.Drawing.Point(94, 16);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(40, 16);
            this.txtAngle.TabIndex = 29;
            this.txtAngle.Text = "0,00";
            this.txtAngle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbComandos
            // 
            this.gbComandos.Controls.Add(this.cbTrajeto);
            this.gbComandos.Controls.Add(this.btnReset);
            this.gbComandos.Controls.Add(this.label4);
            this.gbComandos.Controls.Add(this.btnStep);
            this.gbComandos.Controls.Add(this.cbLasers);
            this.gbComandos.Controls.Add(this.btnRun);
            this.gbComandos.Controls.Add(this.txtInterval);
            this.gbComandos.Location = new System.Drawing.Point(8, 166);
            this.gbComandos.Name = "gbComandos";
            this.gbComandos.Size = new System.Drawing.Size(144, 200);
            this.gbComandos.TabIndex = 26;
            this.gbComandos.TabStop = false;
            this.gbComandos.Text = "Tools:";
            // 
            // cbTrajeto
            // 
            this.cbTrajeto.Location = new System.Drawing.Point(8, 16);
            this.cbTrajeto.Name = "cbTrajeto";
            this.cbTrajeto.Size = new System.Drawing.Size(120, 24);
            this.cbTrajeto.TabIndex = 19;
            this.cbTrajeto.Text = "&Track Path";
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(6, 167);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Rest&art";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Move Interval (ms):";
            // 
            // pbRobot
            // 
            this.pbRobot.BackColor = System.Drawing.Color.Transparent;
            this.pbRobot.Image = ((System.Drawing.Image)(resources.GetObject("pbRobot.Image")));
            this.pbRobot.Location = new System.Drawing.Point(216, 472);
            this.pbRobot.Name = "pbRobot";
            this.pbRobot.Size = new System.Drawing.Size(10, 10);
            this.pbRobot.TabIndex = 11;
            this.pbRobot.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(8, 372);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 89);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hints:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 88);
            this.label5.TabIndex = 10;
            this.label5.Text = "Left click the image to draw passages (white), right click the image to draw wall" +
    "s (black).";
            // 
            // aboutButton
            // 
            this.aboutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutButton.Location = new System.Drawing.Point(41, 473);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(75, 23);
            this.aboutButton.TabIndex = 28;
            this.aboutButton.Text = "About";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 30;
            this.label6.Text = "Speed (%):";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed.Location = new System.Drawing.Point(94, 32);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(40, 16);
            this.txtSpeed.TabIndex = 31;
            this.txtSpeed.Text = "30,00";
            this.txtSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // txtTime
            // 
            this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(94, 48);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(40, 16);
            this.txtTime.TabIndex = 33;
            this.txtTime.Text = "0";
            this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 32;
            this.label8.Text = "Time (s):";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(664, 513);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbComandos);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbRobot);
            this.Controls.Add(this.pbTerrain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fuzzy Auto Guided Vehicle Sample";
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbComandos.ResumeLayout(false);
            this.gbComandos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        // Run one epoch of the Fuzzy Inference System 
        private void DoInference()
        {
            double NewAngle;
            fis.DoInference(Convert.ToDouble(txtRight.Text),
                Convert.ToDouble(txtLeft.Text),
                Convert.ToDouble(txtFront.Text),
                out NewAngle, out Speed);
            txtAngle.Text = NewAngle.ToString("##0.#0");
            Angle += NewAngle;
        }

        // AGV's terrain drawing
        private void pbTerrain_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            pbTerrain.Image = CopyImage( OriginalMap );
            LastX = e.X;
            LastY = e.Y;
        }

        // AGV's terrain drawing
        private void pbTerrain_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            Graphics g = Graphics.FromImage( pbTerrain.Image );

            Color c = Color.Yellow;

            if ( e.Button == MouseButtons.Left )
                c = Color.White;
            else if ( e.Button == MouseButtons.Right )
                c = Color.Black;

            if ( c != Color.Yellow )
            {
                g.FillRectangle( new SolidBrush( c ), e.X - 40, e.Y - 40, 80, 80 );

                LastX = e.X;
                LastY = e.Y;

                g.DrawImage( pbTerrain.Image, 0, 0 );
                OriginalMap = CopyImage( pbTerrain.Image as Bitmap );
                pbTerrain.Refresh( );
                g.Dispose( );
            }

        }

        // Getting sensors measures
        private void GetMeasures( )
        {
            // Getting AGV's position
            pbTerrain.Image = CopyImage( OriginalMap );
            Bitmap b = pbTerrain.Image as Bitmap;
            Point pPos = new Point( pbRobot.Left - pbTerrain.Left + 5, pbRobot.Top - pbTerrain.Top + 5 );

            // AGV on the wall
            if ( ( b.GetPixel( pPos.X, pPos.Y ).R == 0 ) && ( b.GetPixel( pPos.X, pPos.Y ).G == 0 ) && ( b.GetPixel( pPos.X, pPos.Y ).B == 0 ) )
            {
                if ( btnRun.Text != RunLabel )
                {
                    btnRun_Click( btnRun, null );
                }
                string Msg = "The vehicle is on the solid area!";
                MessageBox.Show( Msg, "Error!" );
                throw new Exception( Msg );
            }

            // Getting distances
            Point pFrontObstacle = GetObstacle( pPos, b, -1, 0 );
            Point pLeftObstacle = GetObstacle( pPos, b, 1, 90 );
            Point pRightObstacle = GetObstacle( pPos, b, 1, -90 );

            // Showing beams
            Graphics g = Graphics.FromImage( b );
            if ( cbLasers.Checked )
            {
                g.DrawLine( new Pen( Color.Green, 1 ), pFrontObstacle, pPos );
                g.DrawLine( new Pen( Color.Red, 1 ), pLeftObstacle, pPos );
                g.DrawLine( new Pen( Color.Red, 1 ), pRightObstacle, pPos );
            }

            // Drawing AGV
            if ( btnRun.Text != RunLabel )
            {
                g.FillEllipse( new SolidBrush( Color.Navy ), pPos.X - 5, pPos.Y - 5, 10, 10 );
            }

            g.DrawImage( b, 0, 0 );
            g.Dispose( );

            pbTerrain.Refresh( );

            // Updating distances texts
            txtFront.Text = GetDistance( pPos, pFrontObstacle ).ToString( );
            txtLeft.Text = GetDistance( pPos, pLeftObstacle ).ToString( );
            txtRight.Text = GetDistance( pPos, pRightObstacle ).ToString( );

        }

        // Calculating distances
        private int GetDistance( Point p1, Point p2 )
        {
            return ( Convert.ToInt32( Math.Sqrt( Math.Pow( p1.X - p2.X, 2 ) + Math.Pow( p1.Y - p2.Y, 2 ) ) ) );
        }

        // Finding obstacles
        private Point GetObstacle( Point Start, Bitmap Map, int Inc, int AngleOffset )
        {
            Point p = new Point( Start.X, Start.Y );

            double rad = ( ( Angle + 90 + AngleOffset ) * Math.PI ) / 180;
            int IncX = 0;
            int IncY = 0;
            int Offset = 0;

            while ( ( p.X + IncX >= 0 ) && ( p.X + IncX < Map.Width ) && ( p.Y + IncY >= 0 ) && ( p.Y + IncY < Map.Height ) )
            {
                if ( ( Map.GetPixel( p.X + IncX, p.Y + IncY ).R == 0 ) && ( Map.GetPixel( p.X + IncX, p.Y + IncY ).G == 0 ) && ( Map.GetPixel( p.X + IncX, p.Y + IncY ).B == 0 ) )
                    break;
                Offset += Inc;
                IncX = Convert.ToInt32( Offset * Math.Cos( rad ) );
                IncY = Convert.ToInt32( Offset * Math.Sin( rad ) );
            }
            p.X += IncX;
            p.Y += IncY;

            return p;
        }

        // Copying bitmaps
        private Bitmap CopyImage( Bitmap Src )
        {
            return new Bitmap( Src );
        }


        // Restarting the AGVs simulation
        private void btnReset_Click( object sender, System.EventArgs e )
        {
            Angle = 0;
            pbTerrain.Image = new Bitmap( InitialMap );
            OriginalMap = new Bitmap( InitialMap );
            FirstInference = true;
            pbRobot.Location = InitialPos;
            txtFront.Text = "0";
            txtLeft.Text = "0";
            txtRight.Text = "0";
            txtAngle.Text = "0,00";
        }

        // Moving the AGV
        private void MoveAGV( )
        {
            double rad = ( ( Angle + 90 ) * Math.PI ) / 180;
            int Offset = 0;
            int Inc = -Convert.ToInt32(Speed / 10);

            Offset += Inc;
            int IncX = Convert.ToInt32( Offset * Math.Cos( rad ) );
            int IncY = Convert.ToInt32( Offset * Math.Sin( rad ) );

            // Leaving the track 
            if ( cbTrajeto.Checked )
            {
                Graphics g = Graphics.FromImage( OriginalMap );
                Point p1 = new Point( pbRobot.Left - pbTerrain.Left + pbRobot.Width / 2, pbRobot.Top - pbTerrain.Top + pbRobot.Height / 2 );
                Point p2 = new Point( p1.X + IncX, p1.Y + IncY );
                g.DrawLine( new Pen( new SolidBrush( Color.Blue ) ), p1, p2 );
                g.DrawImage( OriginalMap, 0, 0 );
                g.Dispose( );
            }

            pbRobot.Top = pbRobot.Top + IncY;
            pbRobot.Left = pbRobot.Left + IncX;
        }

        // Starting and stopping the AGV's moviment a
        private void btnRun_Click( object sender, System.EventArgs e )
        {
            Button b = ( sender as Button );

            if ( b.Text == RunLabel )
            {
                b.Text = "&Stop";
                btnStep.Enabled = false;
                btnReset.Enabled = false;
                txtInterval.Enabled = false;
                cbLasers.Enabled = false;
                cbTrajeto.Enabled = false;
                pbRobot.Hide( );
                StartMovement( );
            }
            else
            {
                StopMovement( );
                b.Text = RunLabel;
                btnReset.Enabled = true;
                btnStep.Enabled = true;
                txtInterval.Enabled = true;
                cbLasers.Enabled = true;
                cbTrajeto.Enabled = true;
                pbRobot.Show( );
                pbTerrain.Image = CopyImage( OriginalMap );
                pbTerrain.Refresh( );
            }
        }

        // One step of the AGV
        private void button3_Click( object sender, System.EventArgs e )
        {
            pbRobot.Hide( );
            AGVStep( );
            pbRobot.Show( );
        }

        // Thread for the AGVs movement
        private void StartMovement( )
        {
            thMovement = new Thread( new ThreadStart( MoveCycle ) );
            thMovement.IsBackground = true;
            thMovement.Priority = ThreadPriority.AboveNormal;
            thMovement.Start( );
            Time = 0;
            timer.Enabled = true;
        }

        // Thread main cycle
        private void MoveCycle( )
        {
            try
            {
                while ( Thread.CurrentThread.IsAlive )
                {
                    MethodInvoker mi = new MethodInvoker( AGVStep );
                    this.BeginInvoke( mi );
                    Thread.Sleep( Convert.ToInt32( txtInterval.Text ) );
                }
            }
            catch ( ThreadInterruptedException )
            {
            }
        }

        // One step of the AGV
        private void AGVStep( )
        {
            if ( FirstInference ) GetMeasures( );

            try
            {
                DoInference( );
                MoveAGV( );
                GetMeasures( );
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( ex );
            }
        }

        // Stop background thread
        private void StopMovement( )
        {
            if ( thMovement != null )
            {
                thMovement.Interrupt( );
                thMovement = null;
            }
            timer.Enabled = false;

        }

        // Show About dialog
        private void aboutButton_Click( object sender, EventArgs e )
        {
            AboutForm form = new AboutForm( );

            form.ShowDialog( );
        }

        private void pbTerrain_Click(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Time += 100;
            txtTime.Text = (Time / 1000.0).ToString("#0.#0"); 
        }
    }
}
