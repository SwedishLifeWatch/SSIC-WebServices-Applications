using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class creates a red list criteria string from it's sub parts.
    /// </summary>
    public class RedListCriteriaBuilder
    {
        private Int32 _currentLevel;
        private StringBuilder _criteria;

        /// <summary>
        /// Create a RedListCriteriaBuilder instance.
        /// </summary>
        public RedListCriteriaBuilder()
        {
            _criteria = new StringBuilder();
            _currentLevel = 0;
        }

        /// <summary>
        /// Add criteria part to string, e.g. A, B, C, D or E.
        /// </summary>
        /// <param name="criteriaPart">Part of criteria to add.</param>
        public void AddCriteriaLevel1(String criteriaPart)
        {
            SetLevel(1);
            _criteria.Append(criteriaPart);
        }

        /// <summary>
        /// Add criteria part to string, e.g. 1, 2, 3 or 4.
        /// </summary>
        /// <param name="criteriaPart">Part of criteria to add.</param>
        public void AddCriteriaLevel2(Int32 criteriaPart)
        {
            SetLevel(2);
            _criteria.Append(criteriaPart);
        }

        /// <summary>
        /// Add criteria part to string, e.g. a, b, c or d.
        /// </summary>
        /// <param name="criteriaPart">Part of criteria to add.</param>
        public void AddCriteriaLevel3(String criteriaPart)
        {
            SetLevel(3);
            _criteria.Append(criteriaPart);
        }

        /// <summary>
        /// Add criteria part to string, e.g. i, ii, iii, iv or v.
        /// </summary>
        /// <param name="criteriaPart">Part of criteria to add.</param>
        public void AddCriteriaLevel4(Int32 criteriaPart)
        {
            SetLevel(4);
            switch (criteriaPart)
            {
                case 1:
                    _criteria.Append("i");
                    break;
                case 2:
                    _criteria.Append("ii");
                    break;
                case 3:
                    _criteria.Append("iii");
                    break;
                case 4:
                    _criteria.Append("iv");
                    break;
                case 5:
                    _criteria.Append("v");
                    break;
                default:
                    throw new Exception("Can only handle numbers from 1 to 5.");
            }
        }

        /// <summary>
        /// Set level for criteria part that should be added.
        /// </summary>
        /// <param name="level">Level for next criteria part.</param>
        private void SetLevel(Int32 level)
        {
            if ((level - 2) >= _currentLevel)
            {
                throw new Exception("Can only step on level at the time!");
            }

            if (level > _currentLevel)
            {
                switch (level)
                {
                    case 4:
                        _criteria.Append("(");
                        break;
                }
            }
            else if (level == _currentLevel)
            {
                switch (_currentLevel)
                {
                    case 1:
                        _criteria.Append("; ");
                        break;
                    case 2:
                        _criteria.Append("+");
                        break;
                    case 4:
                        _criteria.Append(",");
                        break;
                }
            }
            else if (level < _currentLevel)
            {
                switch (_currentLevel)
                {
                    case 4:
                        _criteria.Append(")");
                        break;
                }
                switch (level)
                {
                    case 1:
                        _criteria.Append("; ");
                        break;
                    case 2:
                        _criteria.Append("+");
                        break;
                    case 4:
                        _criteria.Append(",");
                        break;
                }
            }
            _currentLevel = level;
        }

        /// <summary>
        /// Get criteria string.
        /// </summary>
        public override string ToString()
        {
            SetLevel(0);
            return _criteria.ToString();
        }
    }
}
