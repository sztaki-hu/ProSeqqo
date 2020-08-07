using SequencePlanner.Phraser.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTemplateValidator
    {
        public bool Validate(LineLikeTemplate template)
        {
            CheckLineList(template);
            CheckLinePrecedences(template);
            CheckContourPrecedences(template);
            return true;
        }

        private void CheckLinePrecedences(LineLikeTemplate template)
        {
            if (template.LinePrecedence != null)
            {
                foreach (var precedence in template.LinePrecedence)
                {
                    bool findBeforeID = false;
                    bool findAfterID = false;
                    foreach (var line in template.LineList)
                    {
                        if (precedence.BeforeID == line.LineID)
                            findBeforeID = true;
                        if (precedence.AfterID == line.LineID)
                            findAfterID = true;
                    }
                    if (!findBeforeID || !findAfterID)
                    {
                        var missingID = -1;
                        if (!findBeforeID)
                            missingID = precedence.BeforeID;
                        if (!findAfterID)
                            missingID = precedence.AfterID;
                        throw new SequencerException("Unknown LineID in LinePrecedences section: " + missingID + ".", "Change the ID for valid LineID or check syntax.");
                    }
                }
            }
            
        }
        private void CheckContourPrecedences(LineLikeTemplate template)
        {
            if(template.ContourPrecedence != null)
            {
                foreach (var precedence in template.ContourPrecedence)
                {
                    bool findBeforeID = false;
                    bool findAfterID = false;
                    foreach (var line in template.LineList)
                    {
                        if (precedence.BeforeID == line.ContourID)
                            findBeforeID = true;
                        if (precedence.AfterID == line.ContourID)
                            findAfterID = true;
                    }
                    if (!findBeforeID || !findAfterID)
                    {
                        var missingID = -1;
                        if (!findBeforeID)
                            missingID = precedence.BeforeID;
                        if (!findAfterID)
                            missingID = precedence.AfterID;
                        throw new SequencerException("Unknown ContourID in ContourPrecedence section: " + missingID + ".", "Change the ID for valid ContourID or check syntax.");
                    }
                }
            }
        }
        private void CheckLineList(LineLikeTemplate template)
        {
            if(template.PositionList!=null)
                foreach (var line in template.LineList)
                {
                    bool findPosA = false;
                    bool findPosB = false;
                    foreach (var position in template.PositionList)
                    {
                        if (line.PositionA == position.ID)
                            findPosA = true;
                        if (line.PositionB == position.ID)
                            findPosB = true;
                    }
                    if (!findPosA || !findPosB)
                    {
                        var missingID = -1;
                        if (!findPosA)
                            missingID = line.PositionA;
                        if (!findPosB)
                            missingID = line.PositionB;
                        throw new SequencerException("Unknown PositionID in LineList section: " + missingID + ".", "Change the ID for valid PositionID or check syntax.");
                    }
                }
        }

    }
}
