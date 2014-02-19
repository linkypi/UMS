namespace SlModel
{
    public class CkbModel
    {
        private bool flag;
        private string text;

        public CkbModel(bool f, string text)
        {
            this.flag = f;
            this.text = text;
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }
    }
}
