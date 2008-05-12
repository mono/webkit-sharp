
TARGETS = \
	funnybrowser.exe

EXTRA_DIST = \
	FunnyBrowser.cs

DEBUGS = $(addsuffix .mdb, $(TARGETS))

assembly=$(ASSEMBLY_NAME).dll

CLEANFILES = $(TARGETS) $(DEBUGS) $(assembly)

references=-r:../sources/$(assembly) -pkg:gtk-sharp-2.0

all: $(TARGETS)

$(assembly): ../sources/$(assembly)
	cp ../sources/$(ASSEMBLY_NAME).dll $(srcdir)

funnybrowser.exe: $(assembly) $(srcdir)/FunnyBrowser.cs
	$(CSC)  /out:$@ $(references) $(srcdir)/FunnyBrowser.cs

