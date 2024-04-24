namespace WarehouseSimulator.View
{
        
    public enum MessageBoxResponse
    {
        CONFIRMED,
        DECLINED,
        CANCELED
    }

    
    public interface MessageBoxTypeSelector
    {
        public string[] GetButtonText();
    }

    public class SimpleMessageBoxTypeSelector : MessageBoxTypeSelector
    {
        private MessageBoxType _type;
        
        public MessageBoxType Type { get; set; }

        public SimpleMessageBoxTypeSelector(MessageBoxType type)
        {
            Type = type;
        }
        
        public enum MessageBoxType
        {
            OK_CANCEL,
            CONFIRM_CANCEL,
            YES_NO
        }

        public string[] GetButtonText()
        {
            switch (Type)
            {
                case MessageBoxType.OK_CANCEL: return new string[]{"OK","Cancel"};
                case MessageBoxType.YES_NO: return new string[] { "Yes", "No" };
                case MessageBoxType.CONFIRM_CANCEL: return new string[] { "Confirm", "Cancel" };
                default: return new string[]{"",""};
            }
        }
    }

    public class ComplexMessageBoxTypeSelector : MessageBoxTypeSelector
    {
        private MessageBoxType _type;
        
        public MessageBoxType Type { get; set; }

        public ComplexMessageBoxTypeSelector(MessageBoxType type)
        {
            Type = type;
        }
        
        public enum MessageBoxType
        {
            CONFIRM_DECLINE_CANCEL
        }
        public string[] GetButtonText()
        {
            switch (Type)
            {
                case MessageBoxType.CONFIRM_DECLINE_CANCEL: return new string[]{"Confirm","Decline","Cancel"};
                default: return new string[]{"","",""};
            }
        }
    }

    public class OneWayMessageBoxTypeSelector : MessageBoxTypeSelector
    {
        private MessageBoxType _type;
        
        public MessageBoxType Type { get; set; }

        public OneWayMessageBoxTypeSelector(MessageBoxType type)
        {
            Type = type;
        }
        public enum MessageBoxType
        {
            OK
        }
        public string[] GetButtonText()
        {
            switch (Type)
            {
                case MessageBoxType.OK: return new string[]{"OK"};
                default: return new string[]{""};
            }
        }
    }
    
}