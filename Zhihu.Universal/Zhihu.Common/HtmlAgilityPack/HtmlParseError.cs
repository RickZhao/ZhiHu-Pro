
using System;

namespace Zhihu.Common.HtmlAgilityPack
{
    /// <summary>
    /// Represents a parsing error found during document parsing.
    /// </summary>
    public class HtmlParseError
    {
        #region Fields

        private HtmlParseErrorCode _code;
        private Int32 _line;
        private Int32 _linePosition;
        private String _reason;
        private String _sourceText;
        private Int32 _streamPosition;

        #endregion

        #region Constructors

        internal HtmlParseError(
            HtmlParseErrorCode code,
            Int32 line,
            Int32 linePosition,
            Int32 streamPosition,
            String sourceText,
            String reason)
        {
            _code = code;
            _line = line;
            _linePosition = linePosition;
            _streamPosition = streamPosition;
            _sourceText = sourceText;
            _reason = reason;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of error.
        /// </summary>
        public HtmlParseErrorCode Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets the line number of this error in the document.
        /// </summary>
        public Int32 Line
        {
            get { return _line; }
        }

        /// <summary>
        /// Gets the column number of this error in the document.
        /// </summary>
        public Int32 LinePosition
        {
            get { return _linePosition; }
        }

        /// <summary>
        /// Gets a description for the error.
        /// </summary>
        public String Reason
        {
            get { return _reason; }
        }

        /// <summary>
        /// Gets the the full text of the line containing the error.
        /// </summary>
        public String SourceText
        {
            get { return _sourceText; }
        }

        /// <summary>
        /// Gets the absolute stream position of this error in the document, relative to the start of the document.
        /// </summary>
        public Int32 StreamPosition
        {
            get { return _streamPosition; }
        }

        #endregion
    }
}
