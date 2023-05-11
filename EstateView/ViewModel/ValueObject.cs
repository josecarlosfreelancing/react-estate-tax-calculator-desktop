namespace EstateView.ViewModel
{
    public class ValueObject
    {
        public ValueObject(object value, string description)
        {
            this.Value = value;
            this.Description = description;
        }

        public object Value { get; private set; }
        
        public string Description { get; private set; }
    }
}
