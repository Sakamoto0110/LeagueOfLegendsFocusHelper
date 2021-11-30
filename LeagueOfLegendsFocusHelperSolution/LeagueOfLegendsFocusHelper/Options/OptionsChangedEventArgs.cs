namespace LeagueOfLegendsFocusHelper.Options
{
    public class OptionsChangedEventArgs
    {
        public OptionsList OldOptions { get; }
        public OptionsList NewOptions { get; }

        public OptionsChangedEventArgs(OptionsList _old, OptionsList _new)
        {
            OldOptions = _old;
            NewOptions = _new;
        }
    }
}