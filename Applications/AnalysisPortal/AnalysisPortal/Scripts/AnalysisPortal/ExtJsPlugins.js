﻿
/**
* Ext.ux.grid.PageSize
*/
Ext.define('Ext.ux.grid.PageSize', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'plugin.pagesize',
    beforeText: 'Show',
    afterText: 'rows/page',
    mode: 'local',
    displayField: 'text',
    valueField: 'value',
    allowBlank: false,
    editable: false,
    //fieldCls: 'mousepointer',    
    typeAhead: false,    
    triggerAction: 'all',
    width: 50,
    maskRe: /[0-9]/,
    /**
    * initialize the paging combo after the pagebar is randered
    */
    init: function (paging) {
        paging.on('afterrender', this.onInitView, this);
    },
    /**
    * create a local store for availabe range of pages
    */
    store: new Ext.data.SimpleStore({
        fields: ['text', 'value'],
        data: [['10', 10], ['25', 25], ['50', 50], ['100', 100], ['500', 500], ['1000', 1000], ['5000', 5000], ['10000', 10000]]
    }),
    /**
    * assing the select and specialkey events for the combobox 
    * after the pagebar is rendered.
    */
    onInitView: function (paging) {
        this.setValue(paging.store.pageSize);
        paging.add('-', this.beforeText, this, this.afterText);
        this.on('select', this.onPageSizeChanged, paging);
        this.on('specialkey', function (combo, e) {
            if (13 === e.getKey()) {
                this.onPageSizeChanged.call(paging, this);
            }
        });
    },
    /**
    * refresh the page when the value is changed
    */
    onPageSizeChanged: function (combo) {
        this.store.pageSize = parseInt(combo.getRawValue(), 10);
        this.store.loadPage(1);
        //this.doRefresh();
    }
});
