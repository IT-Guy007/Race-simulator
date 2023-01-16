using System;
namespace Model {
    public class Section {
        public SectionTypes sectionType { get; set; }

        //Time in section
        public static int SectionLength { get; } = 100;
        public Section(SectionTypes sectionTypes) {
            this.sectionType = sectionTypes;
        }
    }
}

