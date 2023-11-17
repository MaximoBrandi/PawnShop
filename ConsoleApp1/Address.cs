namespace ConsoleApp1
{
    internal class Address
    {
		private string street;
        private int height;
        private int postalCode;

        public string Street
		{
			get { return this.street; }
			set { this.street = value; }
		}
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }
        public int PostalCode
        {
            get { return this.postalCode; }
            set { this.postalCode = value; }
        }

        public Address(string street, int height, int postalCode)
        {
            Street = street;
            Height = height;
            PostalCode = postalCode;
        }

    }
}
