// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Security;

namespace RockWeb.Plugins.org_secc.Tutorials
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Hello World Data Fetch" )]
    [Category( "SECC > Tutorials" )]
    [Description( "Hello World Fetching Data" )]
    [EmailField( "Email", required: false )]
    [LinkedPage( "Related Page" )]
    public partial class HelloWorldFetchingData : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            if ( IsUserAuthorized( Authorization.EDIT ) )
            {
                gPeople.Actions.ShowAdd = true;
                gPeople.Actions.AddClick += gPeople_Add;
            }

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                BindGrid();
            }
        }

        protected void BindGrid()
        {
            var emailValue = GetAttributeValue( "Email" );
            var query = new PersonService( new RockContext() ).Queryable();
            if ( !string.IsNullOrEmpty( emailValue ) )
            {
                query = query.Where( p => p.Email == emailValue );
            }
            else
            {
                query = query.Take( 10 );
            }
            gPeople.DataSource = query.ToList();
            gPeople.DataBind();
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            BindGrid();
        }

        protected void gPeople_RowSelected( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "RelatedPage", "PersonId", ( int ) e.RowKeyValues["Id"] );
        }

        protected void gPeople_Add( object sender, EventArgs e )
        {
            Response.Redirect( "~/NewFamily/" );

            // prevents .NET from quietly throwing ThreadAbortException
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        #endregion

        #region Methods

        // helper functional methods (like BindGrid(), etc.)

        #endregion
    }
}