using System;
namespace Model {
    public class Section {
        public SectionTypes sectionType { get; set; }

        public Section(SectionTypes sectionTypes)
        {
            this.sectionType = sectionTypes;
        }
    }
}

