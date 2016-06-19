﻿#region Copyright © www.ef-automation.com 2015. All right reserved.

// ----------USER  : yu

#endregion Copyright © www.ef-automation.com 2015. All right reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace BaseUtils.Media
{
    public class QRCodeHelper
    {
        private EncodingOptions options = null;
        private BarcodeWriter writer = null;
        private BarcodeReader reader = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QRCodeHelper(int width, int height)
        {
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            reader = new BarcodeReader();
        }

        public Bitmap generateCode(string content)
        {
            Bitmap bitmap = writer.Write(content);
            return bitmap;
        }

        public string decodeCode(Bitmap codeImg)
        {
            Result result = reader.Decode((Bitmap)codeImg);
            return result.Text;
        }
    }
}