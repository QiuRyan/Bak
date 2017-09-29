namespace jinyinmao.Signature.Service
{
    public static class FDDResultCode
    {
        /// <summary>
        ///     业务异常,失败原因见msg
        /// </summary>
        /// <value>The business error.</value>
        public static int BusinessError => 2002;

        /// <summary>
        ///     其他错误,请联系法大大
        /// </summary>
        /// <value>The success.</value>
        public static int OtherError => 2003;

        /// <summary>
        ///     参数确实或者不合法
        /// </summary>
        /// <value>The success.</value>
        public static int ParameterError => 2001;

        /// <summary>
        ///     已签
        /// </summary>
        public static int Signatured => 1;

        /// <summary>
        ///     操作成功
        /// </summary>
        /// <value>The success.</value>
        public static int Success => 1000;

        /// <summary>
        ///     未签
        /// </summary>
        public static int UnSignature => 0;

        /// <summary>
        ///     上传模板操作成功
        /// </summary>
        /// <value>The upload template success.</value>
        public static int UploadTemplateSuccess => 1;
    }
}