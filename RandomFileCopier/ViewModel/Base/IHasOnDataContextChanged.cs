namespace RandomFileCopier.ViewModel.Base
{
    internal interface IHasDataContextSwitchingMethods
    {
        void OnDataContextChanging();

        void OnDataContextChanged();
    }
}