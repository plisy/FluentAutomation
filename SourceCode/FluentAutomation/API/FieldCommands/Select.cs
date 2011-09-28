﻿// <copyright file="Select.cs" author="Brandon Stirnaman">
//     Copyright (c) 2011 Brandon Stirnaman, All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAutomation.API.Enumerations;
using FluentAutomation.API.Providers;

namespace FluentAutomation.API.FieldCommands
{
    /// <summary>
    /// Select Commands
    /// </summary>
    public class Select
    {
        private AutomationProvider _automation = null;
        private string[] _values = null;
        private int[] _selectedIndices = null;
        private SelectMode _selectMode = SelectMode.Value;
        private Expression<Func<string, bool>> _optionMatchingFunc = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="automationProvider">The automation provider.</param>
        /// <param name="optionMatchingFunc">The option matching func.</param>
        /// <param name="selectMode">The select mode.</param>
        public Select(AutomationProvider automationProvider, Expression<Func<string, bool>> optionMatchingFunc, SelectMode selectMode)
        {
            _automation = automationProvider;
            _optionMatchingFunc = optionMatchingFunc;
            _selectMode = selectMode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="automationProvider">The automation provider.</param>
        /// <param name="values">The values.</param>
        /// <param name="selectMode">The select mode.</param>
        public Select(AutomationProvider automationProvider, string[] values, SelectMode selectMode)
        {
            _automation = automationProvider;
            _values = values;
            _selectMode = selectMode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="automationProvider">The automation provider.</param>
        /// <param name="selectedIndices">The selected indices.</param>
        /// <param name="selectMode">The select mode.</param>
        public Select(AutomationProvider automationProvider, int[] selectedIndices, SelectMode selectMode)
        {
            _automation = automationProvider;
            _selectedIndices = selectedIndices;
            _selectMode = selectMode;
        }

        /// <summary>
        /// Selects values/text/indices from the specified field selector.
        /// </summary>
        /// <param name="fieldSelector">The field selector.</param>
        public void From(string fieldSelector)
        {
            From(fieldSelector, MatchConditions.None);
        }

        /// <summary>
        /// Selects values/text/indices from the specified field selector.
        /// </summary>
        /// <param name="fieldSelector">The field selector.</param>
        /// <param name="conditions">The conditions.</param>
        public void From(string fieldSelector, MatchConditions conditions)
        {
            var field = _automation.GetSelectElement(fieldSelector, conditions);
            field.ClearSelectedItems();

            if (_selectMode == SelectMode.Value || _selectMode == SelectMode.Text)
            {
                if (_optionMatchingFunc == null)
                {
                    if (_values.Length == 1)
                    {
                        field.SetValue(_values.First(), _selectMode);
                    }
                    else if (_values.Length > 1)
                    {
                        field.SetValues(_values, _selectMode);
                    }
                }
                else
                {
                    field.SetValues(_optionMatchingFunc, _selectMode);
                }
            }
            else if (_selectMode == SelectMode.Index)
            {
                if (_selectedIndices.Length == 1)
                {
                    field.SetSelectedIndex(_selectedIndices.First());
                }
                else if (_selectedIndices.Length > 1)
                {
                    field.SetSelectedIndices(_selectedIndices);
                }
            }
        }
    }
}