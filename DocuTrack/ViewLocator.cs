using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack;




public class ViewLocator : IDataTemplate

{
    public Control? Build(Object? data)
    {
        if(data is null)
        {
            return null;
        };
    
        var viewName = data.GetType().FullName!.Replace("ViewModel","View",StringComparison.InvariantCulture);
        var type = Type.GetType(viewName);


        if(type is null)
        {
            return null;
        }

    var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = data;
        return control;


    }
                        // SEE ALL DATA FROM BaseViewModel*  
    public bool Match(object? data) => data is BaseViewModel;


}