
pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = $(ASSEMBLY_PC).pc

RAW_API=$(ASSEMBLY_NAME)-api.raw
API=$(ASSEMBLY_NAME)-api.xml
METADATA=$(ASSEMBLY_NAME).metadata

sources = 
#sources = Class.cs

build_sources = AssemblyInfo.cs $(sources)

customs = 
#customs = App.custom Widget.custom Project.custom WidgetClass.custom

CLEANFILES = $(ASSEMBLY_NAME).dll generated-stamp generated/*.cs $(API)

DISTCLEANFILES= AssemblyInfo.cs $(ASSEMBLY_PC).pc $(ASSEMBLY_NAME).dll.config

noinst_DATA = $(ASSEMBLY_NAME).dll

EXTRA_DIST = $(RAW_API) $(sources) $(customs) $(METADATA) \
	AssemblyInfo.cs.in \
	$(ASSEMBLY_NAME).snk \
	$(ASSEMBLY_NAME).dll.config.in \
	$(ASSEMBLY_NAME).pc.in \
	$(ASSEMBLY_NAME)-sources.xml

all: generated-stamp $(ASSEMBLY)

get-sources:
	cp -rf /usr/include/webkit-1.0 .
	@for file in webkit-1.0/webkit/*.h; do                  \
		sed -i -e 's,^WEBKIT_API ,/*WEBKIT_API*/\r,' $$file; \
	done
	@for file in webkit-1.0/JavaScriptCore/*.h; do        \
		sed -i -e 's,^JS_EXPORT ,/*JS_EXPORT*/\r,' $$file; \
	done

api: get-sources
	PERLLIB=$(GTKSHARP_PREFIX)/share/perl5 \
	PATH=$(GTKSHARP_PREFIX)/bin:$$PATH \
	$(GAPI_PARSER) $(ASSEMBLY_NAME)-sources.xml

$(API): $(srcdir)/$(RAW_API) $(srcdir)/$(METADATA)
	cp $(srcdir)/$(RAW_API) $(API)
	chmod u+w $(API)
	$(GAPI_FIXUP) --api=$(srcdir)/$(API) --metadata=$(srcdir)/$(METADATA)

generated-stamp: $(API)
	 $(GAPI_CODEGEN) --generate $(srcdir)/$(API) $(GTK_SHARP_CFLAGS) \
	 	--outdir=generated --customdir=$(srcdir) \
	 	--assembly-name=$(ASSEMBLY_NAME) && touch generated-stamp

$(ASSEMBLY_NAME).dll: $(build_sources) generated-stamp
	$(CSC) -nowarn:169 -unsafe -target:library $(GTK_SHARP_LIBS) \
		$(build_sources) generated/*.cs -out:$(ASSEMBLY_NAME).dll

install-data-local:
	echo "$(GACUTIL) /i $(ASSEMBLY_NAME).dll /f $(GACUTIL_FLAGS)";  \
        $(GACUTIL) /i $(ASSEMBLY_NAME).dll /f $(GACUTIL_FLAGS) || exit 1;

uninstall-local:
	echo "$(GACUTIL) /u $(ASSEMBLY_NAME) $(GACUTIL_FLAGS)"; \
        $(GACUTIL) /u $(ASSEMBLY_NAME) $(GACUTIL_FLAGS) || exit 1;

