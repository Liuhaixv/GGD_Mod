using System.Drawing;
using System.Windows.Forms;

namespace GGD_Hack.MyForms
{
    public class MyMessageBox : Form
    {
        private Label messageLabel;
        private Button okButton;

        public MyMessageBox(string message)
        {
            // 设置窗体的属性
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 创建消息标签
            this.messageLabel = new Label();
            this.messageLabel.AutoSize = true; // 自动调整标签大小以适应文本
            this.messageLabel.Text = message;
            this.messageLabel.Location = new Point(70, 20);
            this.messageLabel.MaximumSize = new Size(5000, 5000); // 设置标签最大宽度
            this.Controls.Add(this.messageLabel);

            // 添加错误图标
            PictureBox errorIcon = new PictureBox();
            errorIcon.Image = SystemIcons.Error.ToBitmap();
            errorIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            errorIcon.Location = new Point(20, 20);
            errorIcon.Size = new Size(40, 40);
            this.Controls.Add(errorIcon);

            // 创建确定按钮
            this.okButton = new Button();
            this.okButton.Text = "确定";
            this.okButton.DialogResult = DialogResult.OK;
            this.okButton.Anchor = AnchorStyles.Bottom; // 将按钮锚定在底部
            this.okButton.Dock = DockStyle.Bottom; // 将按钮放置在底部
            this.okButton.Height = 50;//按钮高度
            this.ClientSize = new Size(500, 300); // 固定窗体大小
            this.Controls.Add(this.okButton); // 在设置 Anchor 和 Dock 属性之后添加按钮

            // 计算窗体的大小
            int labelWidth = this.messageLabel.PreferredWidth;
            int formWidth = labelWidth + 130; // 130 为图标和边框的宽度
            int formHeight = this.messageLabel.Height + this.okButton.Height + 50; // 50 为图标和边框的高度
            this.ClientSize = new Size(formWidth, formHeight);
        }

        public static void Show(string message)
        {
            MyMessageBox messageBox = new MyMessageBox(message);
            messageBox.ShowDialog();
        }
    }
}
