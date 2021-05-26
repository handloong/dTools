using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dTools
{
    public class LoadingLayer
    {
        private LoadingComponent _layer = null;//半透明蒙板层

        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="alpha">透明度</param>
        /// <param name="isShowLoadingImage">是否显示图标</param>
        public void Show(Control control, int alpha = 125, bool isShowLoadingImage = true)
        {
            try
            {
                if (this._layer == null)
                {
                    this._layer = new LoadingComponent(alpha, isShowLoadingImage);
                    control.Controls.Add(this._layer);
                    this._layer.Dock = DockStyle.Fill;
                    this._layer.BringToFront();
                }
                this._layer.Enabled = true;
                this._layer.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public void Hide()
        {
            try
            {
                if (this._layer != null)
                {
                    this._layer.Visible = false;
                    this._layer.Enabled = false;
                }
            }
            catch { }
        }

    }
}
