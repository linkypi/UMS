public class MyEventArgs:System.EventArgs
{
    private object value;

    public object Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}

