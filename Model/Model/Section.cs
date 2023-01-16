using System;
namespace Model {
    public class Section {
        public SectionTypes SectionType { get; set; }

        //Time in section
        public static int SectionLength { get; } = 100;
        public Section(SectionTypes sectionTypes) {
            this.SectionType = sectionTypes;
        }
    }
}

