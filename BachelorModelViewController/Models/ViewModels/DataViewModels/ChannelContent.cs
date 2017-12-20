using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.DataViewModels
{
    public class ChannelContent
    {
        List<Content> contentSummation;

        public ChannelContent()
        {
            contentSummation = new List<Content>();
        }

        public void add(Content c)
        {
            contentSummation.Add(c);
        }
    }

    public class Content
    {
        string name;
        Type type;
        bool isRequired;

        public Content(string n, Type t, bool ir)
        {
            name = n;
            type = t;
            isRequired = ir;
        }
    }
}
