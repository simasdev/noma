using Noma.ApplicationL.Common.Repositories;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.Helpers;
using Noma.WindowsUI.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Noma.WindowsUI.Model
{
    public class Note : ModelBase
    {
        private readonly IEnumerable<Category> categories;

        public Note(IEnumerable<Category> categories)
        {
            this.categories = categories;
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                ChangeState();
            }
        }

        private string content;
        public string Content
        {
            get => content;
            set
            {
                SetProperty(ref content, value);
                ChangeState();
            }
        }

        private bool inTrash;
        public bool InTrash
        {
            get => inTrash;
            set
            {
                SetProperty(ref inTrash, value);
                ChangeState();
            }
        }

        private int? categoryId;
        public int? CategoryId
        {
            get => categoryId;
            set
            {
                SetProperty(ref categoryId, value);

                if(categoryId != null)
                {
                    BackgroundColor = categories.FirstOrDefault(c => c.Id == categoryId)?.Color ?? 0;
                }
              
                ChangeState();
            }
        }

        private int backgroundColor;
        public int BackgroundColor
        {
            get => backgroundColor;
            set
            {
                SetProperty(ref backgroundColor, value);
                RaisePropertyChanged("BackgroundColorBrush");
                RaisePropertyChanged("Color");
                ChangeState();
            }
        }

        public Brush BackgroundColorBrush
        {
            get => CustomColorConverter.GetBrush(BackgroundColor);
        }

        public Color Color
        {
            get => CustomColorConverter.GetColor(BackgroundColor);
        }


        public DateTime LastModifiedAt { get; set; }

    }
}

